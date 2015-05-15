using System;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class Order
    {
        [JsonProperty(PropertyName = "betId")]
        public string BetId { get; set; }

        [JsonProperty(PropertyName = "orderType")]
        public OrderType OrderType { get; set; }

        [JsonProperty(PropertyName = "status")]
        public OrderStatus Status { get; set; }

        [JsonProperty(PropertyName = "persistenceType")]
        public PersistenceType PersistenceType { get; set; }

        [JsonProperty(PropertyName = "side")]
        public Side Side { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "size")]
        public double Size { get; set; }

        [JsonProperty(PropertyName = "bspLiability")]
        public double? BspLiability { get; set; }

        [JsonProperty(PropertyName = "placedDate")]
        public DateTime? PlacedDate { get; set; }

        [JsonProperty(PropertyName = "avgPriceMatched")]
        public double? AvgPriceMatched { get; set; }

        [JsonProperty(PropertyName = "sizeMatched")]
        public double? SizeMatched { get; set; }

        [JsonProperty(PropertyName = "sizeRemaining")]
        public double? SizeRemaining { get; set; }

        [JsonProperty(PropertyName = "sizeLapsed")]
        public double? SizeLapsed { get; set; }

        [JsonProperty(PropertyName = "sizeCancelled")]
        public double? SizeCancelled { get; set; }

        [JsonProperty(PropertyName = "sizeVoided")]
        public double? SizeVoided { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("BetId={0}", this.BetId)
                                      .AppendFormat(" : OrderType={0}", this.OrderType)
                                      .AppendFormat(" : OrderStatus={0}", this.Status)
                                      .AppendFormat(" : PersistenceType={0}", this.PersistenceType)
                                      .AppendFormat(" : Side={0}", this.Side)
                                      .AppendFormat(" : Size@Price={0}@{1}", this.SizeRemaining, this.Price) // instead of simply Size
                                      .AppendFormat(" : BspLiability={0}", this.BspLiability)
                                      .AppendFormat(" : PlacedDate={0}", this.PlacedDate)
                                      .AppendFormat(" : AvgPriceMatched={0}", this.AvgPriceMatched)
                                      .AppendFormat(" : SizeMatched={0}", this.SizeMatched)
                                      .AppendFormat(" : SizeRemaining={0}", this.SizeRemaining)
                                      .AppendFormat(" : SizeLapsed={0}", this.SizeLapsed)
                                      .AppendFormat(" : SizeCancelled={0}", this.SizeCancelled)
                                      .AppendFormat(" : SizeVoided={0}", this.SizeVoided)
                                      .ToString();
        }
    }
}