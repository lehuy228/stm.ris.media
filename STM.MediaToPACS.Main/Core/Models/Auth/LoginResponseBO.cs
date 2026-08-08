using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaToPacs.Core.Models
{
    public class LoginResponseBO : ResponseBO
    {
        [JsonProperty("access_token")]
        public string access_token { get; set; }

        [JsonProperty("expires_in")]
        public int expires_in { get; set; }
    }

    public class ResponseBO
    {
        [JsonProperty("error")]
        public string error { get; set; }

        [JsonProperty("error_description")]
        public string error_description { get; set; }
    }
}
