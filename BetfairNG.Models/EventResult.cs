using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class EventResult
    {
        [JsonProperty(PropertyName = "event")]
        public Event Event { get; set; }

        // documentation says one thing, api result says another
        [JsonProperty(PropertyName = "eventType")]
        public EventType EventType { get; set; }

        [JsonProperty(PropertyName = "marketCount")]
        public int MarketCount { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "EventResult")
                                      .AppendFormat(" : {0}", this.Event)
                                      .AppendFormat(" : MarketCount={0}", this.MarketCount)
                                      .ToString();
        }
    }
}