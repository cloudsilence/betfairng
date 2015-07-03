using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BetfairNG.Models
{
    [JsonConverter(typeof (StringEnumConverter))]
    public enum RunnerStatus
    {
        ACTIVE,
        WINNER,
        LOSER,
        REMOVED_VACANT,
        REMOVED
    }
}