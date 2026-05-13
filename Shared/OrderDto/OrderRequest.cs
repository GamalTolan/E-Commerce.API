using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderDto
{
    public class OrderRequest
    { 
        public string BasketId { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
        public int DeliveryMethodId { get; set; }
    }
}
