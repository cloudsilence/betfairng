using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class PriceSize
    {
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "size")]
        public double Size { get; set; }

        public override string ToString()
        {
            return string.Format("{0}@{1}", this.Size, this.Price);
        }
    }
}