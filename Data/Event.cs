using System;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class Event
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }

        [JsonProperty(PropertyName = "venue")]
        public string Venue { get; set; }

        [JsonProperty(PropertyName = "openDate")]
        public DateTime? OpenDate { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "Event")
                                      .AppendFormat(" : Id={0}", this.Id)
                                      .AppendFormat(" : Name={0}", this.Name)
                                      .AppendFormat(" : CountryCode={0}", this.CountryCode)
                                      .AppendFormat(" : Venue={0}", this.Venue)
                                      .AppendFormat(" : Timezone={0}", this.Timezone)
                                      .AppendFormat(" : OpenDate={0}", this.OpenDate)
                                      .ToString();
        }
    }
}