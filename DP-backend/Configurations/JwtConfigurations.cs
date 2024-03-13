using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_backend.Configurations
{
   public class JwtConfigurations
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public TimeSpan Lifetime { get; set; }
        public int RefreshLifetime { get; set; }
    }
}
