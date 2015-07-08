using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class CancelInstruction
    {
        [JsonProperty(PropertyName = "betId")]
        public string BetId { get; set; }

        [JsonProperty(PropertyName = "sizeReduction")]
        public double SizeReduction { get; set; }
    }
}