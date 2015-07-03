using System.Collections.Generic;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class ClearedOrderSummaryReport
    {
        [JsonProperty(PropertyName = "clearedOrders")]
        public IList<ClearedOrderSummary> ClearedOrders { get; set; }

        [JsonProperty(PropertyName = "moreAvailable")]
        public bool MoreAvailable { get; set; }
    }
}