using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BetfairNG.Models
{
    public class Exception
    {
        // Exception in json-rpc format
        [JsonProperty(PropertyName = "data")]
        public JObject Data { get; set; } // actual exception details

        // Exception in rescript format
        [JsonProperty(PropertyName = "detail")]
        public JObject Detail { get; set; } // actual exception details
    }
}