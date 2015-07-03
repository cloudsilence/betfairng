using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BetfairNG.Models
{
    [JsonConverter(typeof (StringEnumConverter))]
    public enum PersistenceType
    {   
        // Lapse the order at turn-in-play
        LAPSE,
        // Put the order into the auction (SP) at turn-in-play
        PERSIST,
        // Put the order into the auction (SP) at turn-in-play
        MARKET_ON_CLOSE
    }
}