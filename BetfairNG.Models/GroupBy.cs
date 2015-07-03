using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BetfairNG.Models
{
    [JsonConverter(typeof (StringEnumConverter))]
    public enum GroupBy
    {
        EVENT_TYPE,
        EVENT,
        MARKET,
        SIDE,
        BET
    }
}