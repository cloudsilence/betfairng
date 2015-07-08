using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class EventTypeResult
    {
        [JsonProperty(PropertyName = "eventType")]
        public EventType EventType { get; set; }

        [JsonProperty(PropertyName = "marketCount")]
        public int MarketCount { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "EventTypeResult")
                                      .AppendFormat(" : {0}", this.EventType)
                                      .AppendFormat(" : MarketCount={0}", this.MarketCount)
                                      .ToString();
        }
    }
}