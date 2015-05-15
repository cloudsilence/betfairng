using System;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class Match
    {
        [JsonProperty(PropertyName = "betId")]
        public string BetId { get; set; }

        [JsonProperty(PropertyName = "side")]
        public Side Side { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "size")]
        public double Size { get; set; }

        [JsonProperty(PropertyName = "matchDate")]
        public DateTime MatchDate { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat(" : BetId={0}", this.BetId)
                .AppendFormat(" : Side={0}", this.Side)
                .AppendFormat(" : Size@Price={0}@{1}", this.Size, this.Price)
                .AppendFormat(" : MatchDate={0}", this.MatchDate)
                .ToString();
        }
    }
}