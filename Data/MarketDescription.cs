using System;
using System.Text;
using Newtonsoft.Json;

namespace BetfairNG.Data
{
    public class MarketDescription
    {
        [JsonProperty(PropertyName = "persistenceEnabled")]
        public bool IsPersistenceEnabled { get; set; }

        [JsonProperty(PropertyName = "bspMarket")]
        public bool IsBspMarket { get; set; }

        [JsonProperty(PropertyName = "marketTime")]
        public DateTime MarketTime { get; set; }

        [JsonProperty(PropertyName = "suspendTime")]
        public DateTime? SuspendTime { get; set; }

        [JsonProperty(PropertyName = "settleTime")]
        public DateTime? SettleTime { get; set; }

        [JsonProperty(PropertyName = "bettingType")]
        public MarketBettingType BettingType { get; set; }

        [JsonProperty(PropertyName = "turnInPlayEnabled")]
        public bool IsTurnInPlayEnabled { get; set; }

        [JsonProperty(PropertyName = "marketType")]
        public string MarketType { get; set; }

        [JsonProperty(PropertyName = "regulator")]
        public string Regulator { get; set; }

        [JsonProperty(PropertyName = "marketBaseRate")]
        public double MarketBaseRate { get; set; }

        [JsonProperty(PropertyName = "discountAllowed")]
        public bool IsDiscountAllowed { get; set; }

        [JsonProperty(PropertyName = "wallet")]
        public string Wallet { get; set; }

        [JsonProperty(PropertyName = "rules")]
        public string Rules { get; set; }

        [JsonProperty(PropertyName = "rulesHasDate")]
        public bool RulesHasDate { get; set; }

        [JsonProperty(PropertyName = "clarifications")]
        public string Clarifications { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "MarketDescription")
                                      .AppendFormat(" : BspMarket={0}", this.IsBspMarket)
                                      .AppendFormat(" : BettingType={0}", this.BettingType)
                                      .AppendFormat(" : MarketType={0}", this.MarketType)
                                      .AppendFormat(" : MarketTime={0}", this.MarketTime)
                                      .AppendFormat(" : SuspendTime={0}", this.SuspendTime)
                                      .AppendFormat(" : SettleTime={0}", this.SettleTime)
                                      .AppendFormat(" : MarketBaseRate={0}", this.MarketBaseRate)
                                      .AppendFormat(" : IsPersistenceEnabled={0}", this.IsPersistenceEnabled)
                                      .AppendFormat(" : IsTurnInPlayEnabled={0}", this.IsTurnInPlayEnabled)
                                      .AppendFormat(" : IsDiscountAllowed={0}", this.IsDiscountAllowed)
                                      .AppendFormat(" : Regulator={0}", this.Regulator)
                                      .AppendFormat(" : Rules={0}", this.Rules)
                                      .AppendFormat(" : RulesHasDate={0}", this.RulesHasDate)
                                      .AppendFormat(" : Clarifications={0}", this.Clarifications)
                                      .AppendFormat(" : Wallet={0}", this.Wallet)
                                      .ToString();
        }
    }
}