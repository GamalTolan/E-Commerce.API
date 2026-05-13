using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntities
{
    public class OrderItem : BaseEntity<Guid>
    {
        public OrderItem()
        {
            
        }

        public OrderItem(ProductInOrder product, int quentity, decimal price)
        {
            Product = product;
            Quentity = quentity;
            Price = price;
        }

        public ProductInOrder Product { get; set; }
        public int Quentity { get; set; }
        public  decimal Price { get; set; }

    }
}
