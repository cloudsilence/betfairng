using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class LimitOrder
    {
        [JsonProperty(PropertyName = "size")]
        public double Size { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "persistenceType")]
        public PersistenceType PersistenceType { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("Size={0}", this.Size)
                .AppendFormat(" : Price={0}", this.Price)
                .AppendFormat(" : PersistenceType={0}", this.PersistenceType)
                .ToString();
        }
    }
}