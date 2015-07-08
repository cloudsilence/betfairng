using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class CompetitionResult
    {
        [JsonProperty(PropertyName = "competition")]
        public Competition Competition { get; set; }

        [JsonProperty(PropertyName = "marketCount")]
        public int MarketCount { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "CompetitionResult")
                                      .AppendFormat(" : {0}", this.Competition)
                                      .AppendFormat(" : MarketCount={0}", this.MarketCount)
                                      .ToString();
        }
    }
}