using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RateLimit
{
    public class RateLimitResult
    {
        public bool IsAllowed { get; set; }
        public int Limit { get; set; }
        public int Remaining { get; set; }
        public int CurrentCount { get; set; }
        public DateTime WindowResetTime { get; set; }
        public int RetryAfterSeconds { get; set; }
    }
}
