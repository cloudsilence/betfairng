using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class RunnerDescription
    {
        [JsonProperty(PropertyName = "selectionId")]
        public long SelectionId { get; set; }

        [JsonProperty(PropertyName = "runnerName")]
        public string RunnerName { get; set; }

        [JsonProperty(PropertyName = "handicap")]
        public double Handicap { get; set; }

        [JsonProperty(PropertyName = "metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "RunnerDescription")
                                      .AppendFormat(" : SelectionId={0}", this.SelectionId)
                                      .AppendFormat(" : runnerName={0}", this.RunnerName)
                                      .AppendFormat(" : Handicap={0}", this.Handicap)
                                      .AppendFormat(" : Metadata={0}", this.Metadata)
                                      .ToString();
        }
    }
}