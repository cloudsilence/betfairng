using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class ExchangePrices
    {
        [JsonProperty(PropertyName = "availableToBack")]
        public List<PriceSize> AvailableToBack { get; set; }

        [JsonProperty(PropertyName = "availableToLay")]
        public List<PriceSize> AvailableToLay { get; set; }

        [JsonProperty(PropertyName = "tradedVolume")]
        public List<PriceSize> TradedVolume { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder().AppendFormat("{0}", "ExchangePrices");

            if (this.AvailableToBack != null && this.AvailableToBack.Count > 0)
            {
                var idx = 0;
                foreach (var availableToBack in this.AvailableToBack)
                {
                    sb.AppendFormat(" : AvailableToBack[{0}]={1}", idx++, availableToBack);
                }
            }

            if (this.AvailableToLay != null && this.AvailableToLay.Count > 0)
            {
                var idx = 0;
                foreach (var availableToLay in this.AvailableToLay)
                {
                    sb.AppendFormat(" : AvailableToLay[{0}]={1}", idx++, availableToLay);
                }
            }

            if (this.TradedVolume != null && this.TradedVolume.Count > 0)
            {
                var idx = 0;
                foreach (var tradedVolume in this.TradedVolume)
                {
                    sb.AppendFormat(" : TradedVolume[{0}]={1}", idx++, tradedVolume);
                }
            }

            return sb.ToString();
        }
    }
}