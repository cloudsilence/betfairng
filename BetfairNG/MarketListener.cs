using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using BetfairNG.Models;

namespace BetfairNG
{
    public class MarketListener
    {
        private static MarketListener listener;
        private static DateTime lastRequestStart;

        private static DateTime latestDataRequestStart = DateTime.Now;
        private static DateTime latestDataRequestFinish = DateTime.Now;

        private static readonly object LockObj = new object();

        private readonly BetfairClient client;
        private readonly int connectionCount;
        private readonly ConcurrentDictionary<string, IObservable<MarketBook>> markets = new ConcurrentDictionary<string, IObservable<MarketBook>>();
        private readonly ConcurrentDictionary<string, IObserver<MarketBook>> observers = new ConcurrentDictionary<string, IObserver<MarketBook>>();
        private readonly PriceProjection priceProjection;

        private MarketListener(BetfairClient client, PriceProjection priceProjection, int connectionCount)
        {
            this.client = client;
            this.priceProjection = priceProjection;
            this.connectionCount = connectionCount;
            Task.Run(() => this.PollMarketBooks());
        }

        public static MarketListener Create(BetfairClient client, PriceProjection priceProjection, int connectionCount)
        {
            return listener ?? (listener = new MarketListener(client, priceProjection, connectionCount));
        }

        public IObservable<Runner> SubscribeRunner(string marketId, long selectionId)
        {
            var marketTicks = this.SubscribeMarketBook(marketId);

            var observable = Observable.Create((IObserver<Runner> observer) =>
            {
                var subscription = marketTicks.Subscribe(tick =>
                {
                    var runner = tick.Runners.First(c => c.SelectionId == selectionId);

                    // attach the book
                    runner.MarketBook = tick;
                    observer.OnNext(runner);
                });

                return Disposable.Create(subscription.Dispose);
            }).Publish().RefCount();

            return observable;
        }

        public IObservable<MarketBook> SubscribeMarketBook(string marketId)
        {
            IObservable<MarketBook> market;
            if (this.markets.TryGetValue(marketId, out market))
            {
                return market;
            }

            var observable = Observable.Create((IObserver<MarketBook> observer) =>
            {
                this.observers.AddOrUpdate(marketId, observer, (key, existingVal) => existingVal);
                return Disposable.Create(() =>
                {
                    IObserver<MarketBook> ob;
                    IObservable<MarketBook> o;
                    this.markets.TryRemove(marketId, out o);
                    this.observers.TryRemove(marketId, out ob);
                });
            }).Publish().RefCount();

            this.markets.AddOrUpdate(marketId, observable, (key, existingVal) => existingVal);
            return observable;
        }

        // TODO: replace this with the Rx scheduler 
        private void PollMarketBooks()
        {
            for (var i = 0; i < this.connectionCount; i++)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        if (this.markets.Count > 0)
                        {
                            // TODO: look at spinwait or signalling instead of this
                            while (this.connectionCount > 1 && DateTime.Now.Subtract(lastRequestStart).TotalMilliseconds < (1000 / this.connectionCount))
                            {
                                var waitMs = (1000 / this.connectionCount) - (int) DateTime.Now.Subtract(lastRequestStart).TotalMilliseconds;
                                Thread.Sleep(waitMs > 0 ? waitMs : 0);
                            }

                            lock (LockObj) lastRequestStart = DateTime.Now;

                            var book = this.client.ListMarketBook(this.markets.Keys.ToList(), this.priceProjection).Result;

                            if (!book.HasError)
                            {
                                // we may have fresher data than the response to this request
                                if (book.RequestStart < latestDataRequestStart && book.LastByte > latestDataRequestFinish)
                                {
                                    continue;
                                }
                                lock (LockObj)
                                {
                                    latestDataRequestStart = book.RequestStart;
                                    latestDataRequestFinish = book.LastByte;
                                }

                                foreach (var market in book.Response)
                                {
                                    IObserver<MarketBook> o;
                                    if (!this.observers.TryGetValue(market.MarketId, out o))
                                    {
                                        continue;
                                    }

                                    // check to see if the market is finished
                                    if (market.Status == MarketStatus.CLOSED || market.Status == MarketStatus.INACTIVE)
                                    {
                                        o.OnCompleted();
                                    }
                                    else
                                    {
                                        o.OnNext(market);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var observer in this.observers)
                                {
                                    observer.Value.OnError(book.Error);
                                }
                            }
                        }
                        else
                        {
                            // TODO: will die with rx scheduler
                            Thread.Sleep(500);
                        }
                    }
                });

                Thread.Sleep(1000 / this.connectionCount);
            }
        }
    }
}