using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class Runner
    {
        public MarketBook MarketBook { get; set; }

        [JsonProperty(PropertyName = "selectionId")]
        public long SelectionId { get; set; }

        [JsonProperty(PropertyName = "handicap")]
        public double? Handicap { get; set; }

        [JsonProperty(PropertyName = "status")]
        public RunnerStatus Status { get; set; }

        [JsonProperty(PropertyName = "adjustmentFactor")]
        public double? AdjustmentFactor { get; set; }

        [JsonProperty(PropertyName = "lastPriceTraded")]
        public double? LastPriceTraded { get; set; }

        [JsonProperty(PropertyName = "totalMatched")]
        public double TotalMatched { get; set; }

        [JsonProperty(PropertyName = "removalDate")]
        public DateTime? RemovalDate { get; set; }

        [JsonProperty(PropertyName = "sp")]
        public StartingPrices StartingPrices { get; set; }

        [JsonProperty(PropertyName = "ex")]
        public ExchangePrices ExchangePrices { get; set; }

        [JsonProperty(PropertyName = "orders")]
        public List<Order> Orders { get; set; }

        [JsonProperty(PropertyName = "matches")]
        public List<Match> Matches { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder().AppendFormat("SelectionId={0}", this.SelectionId)
                                        .AppendFormat(" : Handicap={0}", this.Handicap)
                                        .AppendFormat(" : Status={0}", this.Status)
                                        .AppendFormat(" : AdjustmentFactor={0}", this.AdjustmentFactor)
                                        .AppendFormat(" : LastPriceTraded={0}", this.LastPriceTraded)
                                        .AppendFormat(" : TotalMatched={0}", this.TotalMatched)
                                        .AppendFormat(" : RemovalDate={0}", this.RemovalDate);

            if (this.StartingPrices != null)
            {
                sb.AppendFormat(": {0}", this.StartingPrices);
            }

            if (this.ExchangePrices != null)
            {
                sb.AppendFormat(": {0}", this.ExchangePrices);
            }

            if (this.Orders != null && this.Orders.Count > 0)
            {
                var idx = 0;
                foreach (var order in this.Orders)
                {
                    sb.AppendFormat(" : Order[{0}]={1}", idx++, order);
                }
            }

            if (this.Matches != null && this.Matches.Count > 0)
            {
                var idx = 0;
                foreach (var match in this.Matches)
                {
                    sb.AppendFormat(" : Match[{0}]={1}", idx++, match);
                }
            }

            return sb.ToString();
        }
    }
}