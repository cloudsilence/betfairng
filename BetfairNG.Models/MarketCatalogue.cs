using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class MarketCatalogue
    {
        [JsonProperty(PropertyName = "marketId")]
        public string MarketId { get; set; }

        [JsonProperty(PropertyName = "marketName")]
        public string MarketName { get; set; }

        [JsonProperty(PropertyName = "isMarketDataDelayed")]
        public bool IsMarketDataDelayed { get; set; }

        [JsonProperty(PropertyName = "description")]
        public MarketDescription Description { get; set; }

        [JsonProperty(PropertyName = "runners")]
        public List<RunnerDescription> Runners { get; set; }

        [JsonProperty(PropertyName = "eventType")]
        public EventType EventType { get; set; }

        [JsonProperty(PropertyName = "event")]
        public Event Event { get; set; }

        [JsonProperty(PropertyName = "competition")]
        public Competition Competition { get; set; }

        public override string ToString()
        {
            // well, don't bother displaying event/event type/competition
            var sb = new StringBuilder().AppendFormat("{0}", "MarketCatalogue")
                                        .AppendFormat(" : Market={0}[{1}]", this.MarketId, this.MarketName)
                                        .AppendFormat(" : IsMarketDataDelayed={0}", this.IsMarketDataDelayed);

            if (this.Description != null)
            {
                sb.AppendFormat(" : {0}", this.Description);
            }

            if (this.Runners != null && this.Runners.Count > 0)
            {
                var idx = 0;
                foreach (var runner in this.Runners)
                {
                    sb.AppendFormat(" : Runner[{0}]={1}", idx++, runner);
                }
            }

            return sb.ToString();
        }
    }
}