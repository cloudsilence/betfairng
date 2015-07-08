using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BetfairNG.Models
{
    [JsonConverter(typeof (StringEnumConverter))]
    public enum OrderType
    {
        // Normal exchange limit order for immediate execution
        LIMIT,
        // Limit order for the auction (SP)
        LIMIT_ON_CLOSE,
        // Market order for the auction (SP)
        MARKET_ON_CLOSE
    }
}