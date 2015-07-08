using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class TransferResponse
    {
        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId { get; set; }
    }
}