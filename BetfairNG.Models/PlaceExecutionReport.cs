using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class PlaceExecutionReport
    {
        [JsonProperty(PropertyName = "status")]
        public ExecutionReportStatus Status { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        public ExecutionReportErrorCode ErrorCode { get; set; }

        [JsonProperty(PropertyName = "marketId")]
        public string MarketId { get; set; }

        [JsonProperty(PropertyName = "instructionReports")]
        public IList<PlaceInstructionReport> InstructionReports { get; set; }

        [JsonProperty(PropertyName = "customerRef")]
        public string CustomerRef { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder()
                .AppendFormat("{0}", "PlaceExecutionReport")
                .AppendFormat(" : Status={0}", this.Status)
                .AppendFormat(" : ErrorCode={0}", this.ErrorCode)
                .AppendFormat(" : MarketId={0}", this.MarketId)
                .AppendFormat(" : CustomerRef={0}", this.CustomerRef);

            if (this.InstructionReports != null && this.InstructionReports.Count > 0)
            {
                var idx = 0;
                foreach (var instructionReport in this.InstructionReports)
                {
                    sb.AppendFormat(" : InstructionReport[{0}]={{{1}}}", idx++, instructionReport);
                }
            }

            return sb.ToString();
        }
    }
}