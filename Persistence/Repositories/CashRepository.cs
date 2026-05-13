using Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CashRepository(IConnectionMultiplexer connection) : ICashRepository
    { 
        private readonly IDatabase _database = connection.GetDatabase();
        async Task<string?> ICashRepository.GetAsync(string cashKey)
            => await _database.StringGetAsync(cashKey);
        

        async Task ICashRepository.SetAsync(string cashKey, string value, TimeSpan timeToLive)
           =>await _database.StringSetAsync(cashKey, value, timeToLive);
    }
}
