using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface ICacheService
    {
        Task<long> IncrementAsync(string key, TimeSpan expiration);
        Task<long?> GetCounterAsync(string key);
        Task<TimeSpan?> GetTimeToLiveAsync(string key);
        Task<bool> KeyExistsAsync(string key);
    }
}
