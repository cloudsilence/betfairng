using Newtonsoft.Json;

namespace BetfairNG
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonRequest
    {
        public JsonRequest()
        {
            this.JsonRpc = "2.0";
        }

        [JsonProperty(PropertyName = "jsonrpc", NullValueHandling = NullValueHandling.Ignore)]
        public string JsonRpc { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "params")]
        public object Params { get; set; }

        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
    }
}