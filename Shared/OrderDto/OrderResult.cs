using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderDto
{
    public class OrderResult 
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public string PaymentStatus { get; set; } 
        public string DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

    }
}
