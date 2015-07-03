using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BetfairNG.Models
{
    [JsonConverter(typeof (StringEnumConverter))]
    public enum MarketStatus
    {
        INACTIVE,
        OPEN,
        SUSPENDED,
        CLOSED
    }
}