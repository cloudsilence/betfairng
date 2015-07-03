using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class StartingPrices
    {
        [JsonProperty(PropertyName = "nearPrice")]
        public double NearPrice { get; set; }

        [JsonProperty(PropertyName = "farPrice")]
        public double FarPrice { get; set; }

        [JsonProperty(PropertyName = "backStakeTaken")]
        public List<PriceSize> BackStakeTaken { get; set; }

        [JsonProperty(PropertyName = "layLiabilityTaken")]
        public List<PriceSize> LayLiabilityTaken { get; set; }

        [JsonProperty(PropertyName = "actualSP")]
        public double ActualSP { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder().AppendFormat("{0}", "StartingPrices")
                                        .AppendFormat(" : NearPrice={0}", this.NearPrice)
                                        .AppendFormat(" : FarPrice={0}", this.FarPrice)
                                        .AppendFormat(" : ActualSP={0}", this.ActualSP);

            if (this.BackStakeTaken != null && this.BackStakeTaken.Count > 0)
            {
                var idx = 0;
                foreach (var backStake in this.BackStakeTaken)
                {
                    sb.AppendFormat(" : BackStake[{0}]={1}", idx++, backStake);
                }
            }

            if (this.LayLiabilityTaken != null && this.LayLiabilityTaken.Count > 0)
            {
                var idx = 0;
                foreach (var layLiability in this.LayLiabilityTaken)
                {
                    sb.AppendFormat(" : LayLiability{0}]={1}", idx++, layLiability);
                }
            }

            return sb.ToString();
        }
    }
}