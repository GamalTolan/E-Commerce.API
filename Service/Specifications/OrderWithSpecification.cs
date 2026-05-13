using Domain.Contracts;
using Domain.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class OrderWithSpecification : Specification<Order>
    {
        public OrderWithSpecification(Guid orderId) : base(o => o.Id == orderId)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);

        }
        public OrderWithSpecification(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderBy(o=> o.OrderDate);



        }
    }
}
