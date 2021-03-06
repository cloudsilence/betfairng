using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class LimitOnCloseOrder
    {
        [JsonProperty(PropertyName = "size")]
        public double Size { get; set; }

        [JsonProperty(PropertyName = "liability")]
        public double Liability { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("Size={0}", this.Size)
                .AppendFormat(" : Liability={0}", this.Liability)
                .ToString();
        }
    }
}