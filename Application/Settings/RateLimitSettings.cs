using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Settings
{
    public class RateLimitSettings
    {
        public int RequestLimit { get; set; } = 100;
        public int WindowInSeconds { get; set; } = 60;
        public bool EnableIpRateLimit { get; set; } = true;
        public bool EnableUserRateLimit { get; set; } = true;
    }
}
