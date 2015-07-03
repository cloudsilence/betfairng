using System.Collections.Generic;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class MarketProfitAndLoss
    {
        [JsonProperty(PropertyName = "marketId")]
        public string MarketId { get; set; }

        [JsonProperty(PropertyName = "commissionApplied")]
        public double CommissionApplied { get; set; }

        [JsonProperty(PropertyName = "profitAndLosses")]
        public List<RunnerProfitAndLoss> ProfitAndLosses { get; set; }
    }
}