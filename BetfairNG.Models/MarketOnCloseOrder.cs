using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class MarketOnCloseOrder
    {
        [JsonProperty(PropertyName = "size")]
        public double Size { get; set; }

        public override string ToString()
        {
            return string.Format("Size={0}", this.Size);
        }
    }
}