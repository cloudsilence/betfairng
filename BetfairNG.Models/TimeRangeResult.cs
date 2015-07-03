using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class TimeRangeResult
    {
        [JsonProperty(PropertyName = "timeRange")]
        public TimeRange TimeRange { get; set; }

        [JsonProperty(PropertyName = "marketCount")]
        public int MarketCount { get; set; }
    }
}