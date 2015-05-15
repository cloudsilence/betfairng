using BetfairNG.Data.Exceptions;
using Newtonsoft.Json;

namespace BetfairNG
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonResponse<T>
    {
        [JsonProperty(PropertyName = "jsonrpc", NullValueHandling = NullValueHandling.Ignore)]
        public string JsonRpc { get; set; }

        [JsonProperty(PropertyName = "result", NullValueHandling = NullValueHandling.Ignore)]
        public T Result { get; set; }

        [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Ignore)]
        public Exception Error { get; set; }

        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }

        [JsonIgnore]
        public bool HasError
        {
            get { return this.Error != null; }
        }
    }
}