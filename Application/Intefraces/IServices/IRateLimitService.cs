using Application.DTOs.RateLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface IRateLimitService
    {
        Task<RateLimitResult> CheckRateLimitAsync(string identifier);
    }
}
