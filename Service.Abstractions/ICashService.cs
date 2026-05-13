using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface ICashService
    {
        public Task<string?> GetAsync(string cashKey);
        public Task SetAsync(string cashKey, object value, TimeSpan timeToLive);
    }
}
