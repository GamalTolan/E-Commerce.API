using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class BasketNotFoundException :NotFoundException
    {
        public BasketNotFoundException(string basketId) : base($"Basket with id {basketId} not found")
        {

        }
    }
}
