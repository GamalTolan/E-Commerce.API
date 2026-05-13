using Domain.Contracts;
using Domain.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class OrderWithPaymentIntentIdSpecification (string paymentIntentId) : Specification<Order>(x=>x.PaymentIntentId == paymentIntentId)
    {
    }
}
