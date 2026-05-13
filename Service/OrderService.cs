using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Service.Abstractions;
using Service.Specifications;
using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderService(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string buyerEmail)
        {
            var address = mapper.Map<ShippingAddress>(orderRequest.ShippingAddress);

            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);
            if (basket is null)
            {
                throw new BasketNotFoundException(orderRequest.BasketId);
            }
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var existingOrderSpecification = new OrderWithPaymentIntentIdSpecification(basket.PaymentIntentId);
            var existingOrder = await orderRepo.GetByIdAsync(existingOrderSpecification);
            if (existingOrder is not null)
            {
                orderRepo.Delete(existingOrder);
            }

            var OrderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product= await unitOfWork.GetRepository<Product,int>().GetByIdAsync(item.Id);
                if (product is null) 
                    throw new ProductNotFoundException(item.Id);
                var productInOrderItem = new ProductInOrder(product.Id, product.Name, product.PictureUrl);
                
                var orderItem = new OrderItem(productInOrderItem, item.Quantity, product.Price);
                
                OrderItems.Add(orderItem);
            }

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderRequest.DeliveryMethodId);
            if (deliveryMethod is null)
            {
                throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);
            }

            var subtotal = OrderItems.Sum(item => item.Price * item.Quentity);

            var order = new Order(buyerEmail, address, OrderItems, deliveryMethod, subtotal);

            await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            await unitOfWork.SaveChangesAsync();
            var orderResult = mapper.Map<OrderResult>(order);
            return orderResult;
        }

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            var mappedDeliveryMethods = mapper.Map<IEnumerable<DeliveryMethodResult>>(deliveryMethods);
            return mappedDeliveryMethods;
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid orderId)
        {
            var orderSpecification = new OrderWithSpecification(orderId);
            var order= await unitOfWork.GetRepository<Order,Guid>().GetByIdAsync(orderSpecification);
            if (order is null)
            {
                throw new OrderNotFoundException(orderId);
            }
            return mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string buyerEmail)
        {
            var orderSpecification = new OrderWithSpecification(buyerEmail);
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(orderSpecification);     
            return mapper.Map<IEnumerable<OrderResult>>(orders);

        }
    }
}
