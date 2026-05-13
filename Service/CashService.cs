using Domain.Contracts;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    public class CashService(ICashRepository cashRepository) : ICashService
    {
        public Task<string?> GetAsync(string cashKey)
        => cashRepository.GetAsync(cashKey);

        public Task SetAsync(string cashKey, object value, TimeSpan timeToLive)
        {
           var serializedValue = JsonSerializer.Serialize(value);
           return cashRepository.SetAsync(cashKey, serializedValue, timeToLive);
        }
    }
}
