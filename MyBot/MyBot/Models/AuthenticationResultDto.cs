using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyBot.Models
{
    public class AuthenticationResultDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public bool Success { get; set; }

        /// <summary>
        /// Expires in seconds.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public string Scope { get; set; }

        public string Error { get; set; }

        [JsonProperty("require_ssl")]
        public bool RequireSsl { get; set; }
    }
}
