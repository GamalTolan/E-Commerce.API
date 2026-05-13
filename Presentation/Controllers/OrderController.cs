using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.OrderDto;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrderController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<OrderResult>> CreateOrder(OrderRequest orderRequest)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var order = await serviceManager.OrderService.CreateOrderAsync(orderRequest, email);
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethods()
        {
            var deliveryMethods = await serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);

        }
        [HttpGet]
        public async Task<ActionResult<OrderResult>> GetOrderById(Guid orderId)
        {
           var order = await serviceManager.OrderService.GetOrderByIdAsync(orderId);
            
           return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetOrdersByEmail()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var orders = await serviceManager.OrderService.GetOrdersByEmailAsync(email);
            return Ok(orders);
        }
    }
}  