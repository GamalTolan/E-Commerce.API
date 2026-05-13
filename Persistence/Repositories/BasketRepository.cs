using Domain.Contracts;
using Domain.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database= connection.GetDatabase();
        public async Task<bool> DeleteBasketAsync(string basketId)
        =>await _database.KeyDeleteAsync(basketId);

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);
            if (basket.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var serializedBasket = JsonSerializer.Serialize(basket);
            var createdOrUpdated = await _database.StringSetAsync(basket.Id, serializedBasket, timeToLive ?? TimeSpan.FromDays(30));
            return createdOrUpdated ? await GetBasketAsync(basket.Id) : null;
        }
    }
}
