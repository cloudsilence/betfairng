using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class PriceSize
    {
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "size")]
        public double Size { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}@{1}", this.Size, this.Price)
                                      .ToString();
        }
    }
}