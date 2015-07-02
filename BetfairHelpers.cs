using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetfairNG.Data;

namespace BetfairNG
{
    public static class BetfairHelpers
    {
        public static MarketFilter HorseRaceFilter(string country = null)
        {
            var marketFilter = new MarketFilter
            {
                EventTypeIds = new HashSet<string> { "7" },
                MarketStartTime = new TimeRange
                {
                    From = DateTime.Now,
                    To = DateTime.Now.AddDays(1)
                }
            };

            if (country != null)
            {
                marketFilter.MarketCountries = new HashSet<string> { country };
            }

            marketFilter.MarketTypeCodes = new HashSet<String> { "WIN" };

            return marketFilter;
        }

        public static PriceProjection HorseRacePriceProjection()
        {
            ISet<PriceData> priceData = new HashSet<PriceData>();

            // Get all prices from the exchange
            priceData.Add(PriceData.EX_TRADED);
            priceData.Add(PriceData.EX_ALL_OFFERS);

            var priceProjection = new PriceProjection { PriceData = priceData };
            return priceProjection;
        }

        public static PriceProjection CricketPriceProjection()
        {
            ISet<PriceData> priceData = new HashSet<PriceData>();

            // Get all prices from the exchange
            priceData.Add(PriceData.EX_TRADED);
            priceData.Add(PriceData.EX_ALL_OFFERS);

            var priceProjection = new PriceProjection { PriceData = priceData };
            return priceProjection;
        }

        public static ISet<MarketProjection> HorseRaceProjection()
        {
            ISet<MarketProjection> marketProjections = new HashSet<MarketProjection>();
            marketProjections.Add(MarketProjection.RUNNER_METADATA);
            marketProjections.Add(MarketProjection.MARKET_DESCRIPTION);
            marketProjections.Add(MarketProjection.EVENT);

            return marketProjections;
        }

        public static ISet<MarketProjection> CricketProjection()
        {
            ISet<MarketProjection> marketProjections = new HashSet<MarketProjection>();
            marketProjections.Add(MarketProjection.MARKET_DESCRIPTION);
            marketProjections.Add(MarketProjection.EVENT);

            return marketProjections;
        }

        public static double GetMarketEfficiency(IEnumerable<double> odds)
        {
            var total = odds.Sum(c => 1.0 / c);
            return 1.0 / total;
        }

        public static double Best(this List<PriceSize> prices)
        {
            if (prices.Count > 0)
            {
                return prices.First().Price;
            }

            return 0.0;
        }

        public static List<Order> Backs(this IEnumerable<Order> orders)
        {
            return orders.Where(c => c.Side == Side.BACK).ToList();
        }

        public static List<Order> Lays(this IEnumerable<Order> orders)
        {
            return orders.Where(c => c.Side == Side.LAY).ToList();
        }

        public static List<T> Copy<T>(this List<T> list)
        {
            var newList = new List<T>();
            for (var i = 0; i < list.Count; i++)
            {
                newList.Add(list[i]);
            }

            return newList;
        }

        public static string MarketBookConsole(
            MarketCatalogue marketCatalogue,
            MarketBook marketBook,
            IEnumerable<RunnerDescription> runnerDescriptions,
            Func<RunnerDescription, Runner, string> backSide = null,
            Func<RunnerDescription, Runner, string> laySide = null)
        {
            var nearestBacks = marketBook.Runners
                                         .Where(c => c.Status == RunnerStatus.ACTIVE)
                                         .Select(c => c.ExchangePrices.AvailableToBack.Count > 0 ? c.ExchangePrices.AvailableToBack.First().Price : 0.0);
            var nearestLays = marketBook.Runners
                                        .Where(c => c.Status == RunnerStatus.ACTIVE)
                                        .Select(c => c.ExchangePrices.AvailableToLay.Count > 0 ? c.ExchangePrices.AvailableToLay.First().Price : 0.0);

            var timeToJump = Convert.ToDateTime(marketCatalogue.Event.OpenDate);
            var timeRemainingToJump = timeToJump.Subtract(DateTime.UtcNow);

            var sb = new StringBuilder()
                .AppendFormat("{0} {1}", marketCatalogue.Event.Name, marketCatalogue.MarketName)
                .AppendFormat(" : {0}% {1}%", GetMarketEfficiency(nearestBacks).ToString("0.##"), GetMarketEfficiency(nearestLays).ToString("0.##"))
                .AppendFormat(" : Status={0}", marketBook.Status)
                .AppendFormat(" : IsInplay={0}", marketBook.IsInplay)
                .AppendFormat(" : Runners={0}", marketBook.NumberOfActiveRunners)
                .AppendFormat(" : Matched={0}", marketBook.TotalMatched.ToString("C0"))
                .AppendFormat(" : Avail={0}", marketBook.TotalAvailable.ToString("C0"));
            sb.AppendLine();
            sb.AppendFormat("Time To Jump: {0}h {1}:{2}",
                timeRemainingToJump.Hours + (timeRemainingToJump.Days * 24),
                timeRemainingToJump.Minutes.ToString("##"),
                timeRemainingToJump.Seconds.ToString("##"));
            sb.AppendLine();

            if (marketBook.Runners != null && marketBook.Runners.Count > 0)
            {
                foreach (var runner in marketBook.Runners.Where(c => c.Status == RunnerStatus.ACTIVE))
                {
                    var runnerName = runnerDescriptions != null ? runnerDescriptions.FirstOrDefault(c => c.SelectionId == runner.SelectionId) : null;
                    var bsString = backSide != null ? backSide(runnerName, runner) : "";
                    var lyString = laySide != null ? laySide(runnerName, runner) : "";

                    var consoleRunnerName = runnerName != null ? runnerName.RunnerName : "null";

                    sb.AppendLine(string.Format("{0} {9} [{1}] {2},{3},{4}  ::  {5},{6},{7} [{8}] {10}",
                        consoleRunnerName.PadRight(25),
                        runner.ExchangePrices.AvailableToBack.Sum(a => a.Size).ToString("0").PadLeft(7),
                        runner.ExchangePrices.AvailableToBack.Count > 2 ? runner.ExchangePrices.AvailableToBack[2].Price.ToString("0.00").PadLeft(6) : "  0.00",
                        runner.ExchangePrices.AvailableToBack.Count > 1 ? runner.ExchangePrices.AvailableToBack[1].Price.ToString("0.00").PadLeft(6) : "  0.00",
                        runner.ExchangePrices.AvailableToBack.Count > 0 ? runner.ExchangePrices.AvailableToBack[0].Price.ToString("0.00").PadLeft(6) : "  0.00",
                        runner.ExchangePrices.AvailableToLay.Count > 0 ? runner.ExchangePrices.AvailableToLay[0].Price.ToString("0.00").PadLeft(6) : "  0.00",
                        runner.ExchangePrices.AvailableToLay.Count > 1 ? runner.ExchangePrices.AvailableToLay[1].Price.ToString("0.00").PadLeft(6) : "  0.00",
                        runner.ExchangePrices.AvailableToLay.Count > 2 ? runner.ExchangePrices.AvailableToLay[2].Price.ToString("0.00").PadLeft(6) : "  0.00",
                        runner.ExchangePrices.AvailableToLay.Sum(a => a.Size).ToString("0").PadLeft(7),
                        bsString,
                        lyString));
                }
            }

            return sb.ToString();
        }

        public static string ToStringRunnerName(IEnumerable<RunnerDescription> descriptions, IEnumerable<Runner> runners)
        {
            var builder = new StringBuilder();

            foreach (var runner in runners)
            {
                var nameRunner = descriptions.First(c => c.SelectionId == runner.SelectionId);

                builder.AppendLine(string.Format("{0}\t [{1}] {2},{3},{4}  ::  {5},{6},{7} [{8}]",
                    nameRunner.RunnerName.PadRight(25),
                    runner.ExchangePrices.AvailableToBack.Sum(a => a.Size).ToString().PadLeft(7),
                    runner.ExchangePrices.AvailableToBack.Count > 2 ? runner.ExchangePrices.AvailableToBack[2].Price.ToString("0.00").PadLeft(6) : "  0.00",
                    runner.ExchangePrices.AvailableToBack.Count > 1 ? runner.ExchangePrices.AvailableToBack[1].Price.ToString("0.00").PadLeft(6) : "  0.00",
                    runner.ExchangePrices.AvailableToBack.Count > 0 ? runner.ExchangePrices.AvailableToBack[0].Price.ToString("0.00").PadLeft(6) : "  0.00",
                    runner.ExchangePrices.AvailableToLay.Count > 0 ? runner.ExchangePrices.AvailableToLay[0].Price.ToString("0.00").PadLeft(6) : "  0.00",
                    runner.ExchangePrices.AvailableToLay.Count > 1 ? runner.ExchangePrices.AvailableToLay[1].Price.ToString("0.00").PadLeft(6) : "  0.00",
                    runner.ExchangePrices.AvailableToLay.Count > 2 ? runner.ExchangePrices.AvailableToLay[2].Price.ToString("0.00").PadLeft(6) : "  0.00",
                    runner.ExchangePrices.AvailableToLay.Sum(a => a.Size).ToString().PadLeft(7)));
            }

            return builder.ToString();
        }
    }
}