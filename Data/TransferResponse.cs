using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class TransferResponse
    {
        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId { get; set; }
    }
}