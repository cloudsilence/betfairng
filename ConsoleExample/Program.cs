using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using BetfairNG;
using BetfairNG.Data;

// This example pulls the latest horse races in the UK markets
// and displays them to the console.
namespace ConsoleExample
{
    public static class Program
    {
        private static readonly ConcurrentQueue<MarketCatalogue> Markets = new ConcurrentQueue<MarketCatalogue>();

        public static void Main()
        {
            var settings = new Settings();
            var client = new BetfairClient(Exchange.UK, settings.AppKey);
            client.Login(settings.CertificateLocation, settings.CertificatePassword, settings.Username, settings.Password);

            // find all the upcoming UK horse races (EventTypeId 7)
            var marketFilter = new MarketFilter
            {
                EventTypeIds = new HashSet<string> { "7" },
                MarketStartTime = new TimeRange
                {
                    From = DateTime.Now,
                    To = DateTime.Now.AddDays(2)
                },
                MarketTypeCodes = new HashSet<String> { "WIN" }
            };

            Console.WriteLine("BetfairClient.ListTimeRanges()");
            var timeRanges = client.ListTimeRanges(marketFilter, TimeGranularity.HOURS).Result;
            if (timeRanges.HasError)
            {
                throw new ApplicationException();
            } 
            
            Console.WriteLine("BetfairClient.ListEventTypes()");
            var eventTypes = client.ListEventTypes(new MarketFilter()).Result;
            if (eventTypes.HasError)
            {
                throw new ApplicationException();
            }

            Console.WriteLine("BetfairClient.ListCurrentOrders()");
            var currentOrders = client.ListCurrentOrders().Result;
            if (currentOrders.HasError)
            {
                throw new ApplicationException();
            }

            Console.WriteLine("BetfairClient.ListVenues()");
            var venues = client.ListVenues(marketFilter).Result;
            if (venues.HasError)
            {
                throw new ApplicationException();
            }

            Console.WriteLine("BetfairClient.GetAccountDetails()");
            var accountDetails = client.GetAccountDetails().Result;
            if (accountDetails.HasError)
            {
                throw new ApplicationException();
            }

            Console.WriteLine("BetfairClient.GetAccountStatement()");
            var accountStatement = client.GetAccountStatement().Result;
            if (accountStatement.HasError)
            {
                throw new ApplicationException();
            }

            Console.Write("BetfairClient.GetAccountFunds() ");
            var acc = client.GetAccountFunds(Wallet.UK).Result;
            if (acc.HasError)
            {
                throw new ApplicationException();
            }
            Console.WriteLine(acc.Response.AvailableToBetBalance);

            Console.WriteLine("BetfairClient.ListClearedOrders()");
            var clearedOrders = client.ListClearedOrders(BetStatus.SETTLED).Result;
            if (clearedOrders.HasError)
            {
                throw new ApplicationException();
            }

            var marketCatalogues = client.ListMarketCatalogue(
                BFHelpers.HorseRaceFilter(),
                BFHelpers.HorseRaceProjection(),
                MarketSort.FIRST_TO_START,
                25).Result.Response;

            marketCatalogues.ForEach(c =>
            {
                Markets.Enqueue(c);
                Console.WriteLine(c.MarketName);
            });

            Console.WriteLine();

            var marketListener = MarketListener.Create(client, BFHelpers.HorseRacePriceProjection(), 1);

            while (Markets.Count > 0)
            {
                var waitHandle = new AutoResetEvent(false);
                MarketCatalogue marketCatalogue;
                Markets.TryDequeue(out marketCatalogue);

                var marketSubscription = marketListener.SubscribeMarketBook(marketCatalogue.MarketId)
                                                       .SubscribeOn(Scheduler.Default)
                                                       .Subscribe(
                                                           tick => Console.WriteLine(BFHelpers.MarketBookConsole(marketCatalogue, tick, marketCatalogue.Runners)),
                                                           () =>
                                                           {
                                                               Console.WriteLine("Market finished");
                                                               waitHandle.Set();
                                                           });

                waitHandle.WaitOne();
                marketSubscription.Dispose();
            }

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}