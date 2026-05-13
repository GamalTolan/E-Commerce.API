using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Service.Abstractions;
using Service.Specifications;
using Shared.BasketDtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PaymentService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IMapper mapper, IConfiguration configuration) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = configuration.GetRequiredSection("Stripe")["SecretKey"];
            var basket = await basketRepository.GetBasketAsync(basketId);
            if (basket is null)
                throw new BasketNotFoundException(basketId);
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Domain.Entities.Product, int>().GetByIdAsync(item.Id);
                if (product is null)
                    throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }
            if (!basket.DeliveryMethodId.HasValue)
                throw new ArgumentNullException();

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod is null)
                throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;
            var amount = (long)(basket.Items.Sum(x => x.Quantity * x.Price) + basket.ShippingPrice) * 100;
            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                var intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = amount
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await basketRepository.UpdateBasketAsync(basket);
            return mapper.Map<BasketDto>(basket);
        }

        public async Task UpdateOrderPaymentStatusAsync(string request, string stripeHeader)
        {
            var stripeEvent = EventUtility.ConstructEvent(request, stripeHeader, configuration.GetRequiredSection("Stripe")["WebhookSecret"]);
            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                  await  UpdateOrderReceivedAsync(paymentIntent.Id);
                    break;

                case EventTypes.PaymentIntentPaymentFailed:
                  await UpdateOrderReceivedAsync(paymentIntent.Id); 
                    break;

            }


        }


        private async Task UpdateOrderReceivedAsync(string paymentIntentId)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderWithPaymentIntentIdSpecification(paymentIntentId));
           
            order.PaymentStatus = OrderPaymentStatus.PaymentReceived;
            unitOfWork.GetRepository<Order, Guid>().Update(order);
            await unitOfWork.SaveChangesAsync();
        }
            private async Task UpdateOrderFailedAsync(string paymentIntentId)
            {
                var order = await unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderWithPaymentIntentIdSpecification(paymentIntentId));
                order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
                unitOfWork.GetRepository<Order, Guid>().Update(order);
                await unitOfWork.SaveChangesAsync();
        }
    }
}