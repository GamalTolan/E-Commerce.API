using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IOrderService
    {
        public Task<OrderResult> GetOrderByIdAsync(Guid orderId);
        public Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string buyerEmail);
        public Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string buyerEmail);
        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();
    }
}
