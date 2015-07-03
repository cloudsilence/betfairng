using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Models
{
    public class PlaceInstruction
    {
        [JsonProperty(PropertyName = "orderType")]
        public OrderType OrderType { get; set; }

        [JsonProperty(PropertyName = "selectionId")]
        public long SelectionId { get; set; }

        [JsonProperty(PropertyName = "handicap")]
        public double? Handicap { get; set; }

        [JsonProperty(PropertyName = "side")]
        public Side Side { get; set; }

        [JsonProperty(PropertyName = "limitOrder")]
        public LimitOrder LimitOrder { get; set; }

        [JsonProperty(PropertyName = "limitOnCloseOrder")]
        public LimitOnCloseOrder LimitOnCloseOrder { get; set; }

        [JsonProperty(PropertyName = "marketOnCloseOrder")]
        public MarketOnCloseOrder MarketOnCloseOrder { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder()
                .AppendFormat("OrderType={0}", this.OrderType)
                .AppendFormat(" : SelectionId={0}", this.SelectionId)
                .AppendFormat(" : Handicap={0}", this.Handicap)
                .AppendFormat(" : Side={0}", this.Side);

            switch (this.OrderType)
            {
                case OrderType.LIMIT:
                    sb.AppendFormat(" : LimitOrder={0}", this.LimitOrder);
                    break;
                case OrderType.LIMIT_ON_CLOSE:
                    sb.AppendFormat(" : LimitOnCloseOrder={0}", this.LimitOnCloseOrder);
                    break;
                case OrderType.MARKET_ON_CLOSE:
                    sb.AppendFormat(" : MarketOnCloseOrder={0}", this.MarketOnCloseOrder);
                    break;
            }

            return sb.ToString();
        }
    }
}