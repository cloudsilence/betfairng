using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class EventType
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "EventType")
                                      .AppendFormat(" : Id={0}", this.Id)
                                      .AppendFormat(" : Name={0}", this.Name)
                                      .ToString();
        }
    }
}