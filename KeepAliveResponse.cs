using Newtonsoft.Json;

namespace BetfairNG
{
    public class KeepAliveResponse
    {
        [JsonProperty(PropertyName = "token")]
        public string SessionToken { get; set; }

        [JsonProperty(PropertyName = "product")]
        public string Product { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}