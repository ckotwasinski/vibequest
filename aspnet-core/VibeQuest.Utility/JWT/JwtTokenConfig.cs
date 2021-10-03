using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VibeQuest.Utility.JWT
{
    public class JwtTokenConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }

    public class JwtConfigurations
    {
        public static readonly string JwtSectionName = "JwtTokenConfig";
        public static readonly int DefaultAccessTokenExpiration = 30;
        public static readonly int DefaultRefreshTokenExpiration = 90;
    }
}
