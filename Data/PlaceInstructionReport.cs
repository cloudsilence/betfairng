using System;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class PlaceInstructionReport
    {
        [JsonProperty(PropertyName = "status")]
        public InstructionReportStatus Status { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        public InstructionReportErrorCode ErrorCode { get; set; }

        [JsonProperty(PropertyName = "instruction")]
        public PlaceInstruction Instruction { get; set; }

        [JsonProperty(PropertyName = "betId")]
        public string BetId { get; set; }

        [JsonProperty(PropertyName = "placedDate")]
        public DateTime? PlacedDate { get; set; }

        [JsonProperty(PropertyName = "averagePriceMatched")]
        public double? AveragePriceMatched { get; set; }

        [JsonProperty(PropertyName = "sizeMatched")]
        public double? SizeMatched { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("Status={0}", this.Status)
                .AppendFormat(" : ErrorCode={0}", this.ErrorCode)
                .AppendFormat(" : Instruction={{{0}}}", this.Instruction)
                .AppendFormat(" : BetId={0}", this.BetId)
                .AppendFormat(" : PlacedDate={0}", this.PlacedDate)
                .AppendFormat(" : AveragePriceMatched={0}", this.AveragePriceMatched)
                .AppendFormat(" : SizeMatched={0}", this.SizeMatched)
                .ToString();
        }
    }
}