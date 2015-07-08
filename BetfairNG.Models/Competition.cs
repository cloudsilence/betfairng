using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class Competition
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "Competition")
                                      .AppendFormat(" : Id={0}", this.Id)
                                      .AppendFormat(" : Name={0}", this.Name)
                                      .ToString();
        }
    }
}