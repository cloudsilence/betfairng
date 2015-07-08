using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BetfairNG
{
    public class Network
    {
        private static readonly TraceSource TraceSource = new TraceSource("BetfairNG.Network");

        public string UserAgent { get; set; }
        public string Host { get; set; }
        public string AppKey { get; set; }
        public string SessionToken { get; set; }
        public int TimeoutMilliseconds { get; set; }
        public int RetryCount { get; set; }
        public bool GZipCompress { get; set; }
        public Action PreRequestAction { get; set; }
        public WebProxy Proxy { get; set; }

        public Network() : this(null, null)
        {
        }

        public Network(string appKey, string sessionToken, Action preRequestAction = null, bool gzipCompress = true, WebProxy proxy = null)
        {
            this.Host = string.Empty;
            this.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            this.TimeoutMilliseconds = 10000;
            this.AppKey = appKey;
            this.SessionToken = sessionToken;
            this.GZipCompress = gzipCompress;
            this.PreRequestAction = preRequestAction;
            this.Proxy = proxy;
        }

        public BetfairServerResponse<KeepAliveResponse> KeepAliveSynchronous()
        {
            var request = (HttpWebRequest) WebRequest.Create("https://identitysso.betfair.com/api/keepAlive");
            request.UseDefaultCredentials = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("X-Application", this.AppKey);
            request.Headers.Add("X-Authentication", this.SessionToken);
            request.Accept = "application/json";

            if (this.Proxy != null)
            {
                request.Proxy = this.Proxy;
            }

            TraceSource.TraceInformation("KeepAlive");

            var requestStart = DateTime.Now;
            var watch = new Stopwatch();
            watch.Start();

            using (var stream = request.GetResponse().GetResponseStream())
            {
                if (stream == null)
                {
                    return null;
                }

                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    var lastByte = DateTime.Now;
                    var response = JsonConvert.Deserialize<KeepAliveResponse>(reader.ReadToEnd());
                    watch.Stop();

                    TraceSource.TraceInformation("KeepAlive finish: {0}ms", watch.ElapsedMilliseconds);

                    return new BetfairServerResponse<KeepAliveResponse>
                    {
                        HasError = !string.IsNullOrWhiteSpace(response.Error),
                        Response = response,
                        LastByte = lastByte,
                        RequestStart = requestStart
                    };
                }
            }
        }

        public Task<BetfairServerResponse<T>> Invoke<T>(Exchange exchange, Endpoint endpoint, string method, IDictionary<string, object> args = null)
        {
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new ArgumentNullException("method");
            }

            TraceSource.TraceInformation("Network: {0}, {1}", FormatEndpoint(endpoint), method);

            var requestStart = DateTime.Now;
            var watch = new Stopwatch();
            watch.Start();

            var url = exchange == Exchange.AUS ? "https://api-au.betfair.com/exchange" : "https://api.betfair.com/exchange";

            if (endpoint == Endpoint.Betting)
            {
                url += "/betting/json-rpc/v1";
            }
            else
            {
                url += "/account/json-rpc/v1";
            }

            var call = new JsonRequest { Method = method, Id = 1, Params = args };
            var requestData = JsonConvert.Serialize(call);
            var response = this.Request(url, requestData, "application/json-rpc", this.AppKey, this.SessionToken);
            var result = response.ContinueWith(c =>
            {
                var lastByte = DateTime.Now;
                var jsonResponse = JsonConvert.Deserialize<JsonResponse<T>>(c.Result);

                watch.Stop();
                
                TraceSource.TraceInformation("Network finish: {0}ms, {1}, {2}", watch.ElapsedMilliseconds, FormatEndpoint(endpoint), method);

                return ToResponse(jsonResponse, requestStart, lastByte, watch.ElapsedMilliseconds);
            });

            return result;
        }

        private Task<string> Request(string url, string requestPostData, string contentType, string appKey, string sessionToken)
        {
            if (this.PreRequestAction != null)
            {
                this.PreRequestAction();
            }

            var request = (HttpWebRequest) WebRequest.Create(url);
            var postData = Encoding.UTF8.GetBytes(requestPostData);
            request.Method = "POST";
            request.ContentType = contentType;

            if (!string.IsNullOrWhiteSpace(appKey))
            {
                request.Headers.Add("X-Application", appKey);
            }

            if (!string.IsNullOrWhiteSpace(sessionToken))
            {
                request.Headers.Add("X-Authentication", sessionToken);
            }

            request.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8");
            request.AllowAutoRedirect = true;
            request.ContentLength = postData.Length;
            request.KeepAlive = true;

            if (this.Proxy != null)
            {
                request.Proxy = this.Proxy;
            }

            if (this.GZipCompress)
            {
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            }

            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            request.UserAgent = this.UserAgent;
            request.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");

            var uri = new Uri(url);
            request.Host = uri.Host;

            if (this.TimeoutMilliseconds != 0)
            {
                request.Timeout = this.TimeoutMilliseconds;
            }

            var result = Task.Factory.FromAsync(
                request.BeginGetRequestStream,
                asyncResult => request.EndGetRequestStream(asyncResult),
                null);

            var continuation = result
                .ContinueWith(stream =>
                {
                    stream.Result.Write(postData, 0, postData.Length);
                    var task = Task.Factory.FromAsync(
                        request.BeginGetResponse,
                        asyncResult => request.EndGetResponse(asyncResult),
                        null);

                    return task.ContinueWith(t => GetResponseHtml((HttpWebResponse) t.Result));
                }).Unwrap();

            return continuation;
        }

        private static BetfairServerResponse<T> ToResponse<T>(JsonResponse<T> response, DateTime requestStart, DateTime lastByteStamp, long latency)
        {
            return new BetfairServerResponse<T>
            {
                Error = BetfairServerException.ToClientException(response.Error),
                HasError = response.HasError,
                Response = response.Result,
                LastByte = lastByteStamp,
                RequestStart = requestStart
            };
        }

        private static string GetResponseHtml(HttpWebResponse response)
        {
            var html = string.Empty;

            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                {
                    return html;
                }

                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (var gzipStream = new GZipStream(responseStream, CompressionMode.Decompress))
                    {
                        using (var streamReader = new StreamReader(gzipStream, Encoding.Default))
                        {
                            html = streamReader.ReadToEnd();
                        }
                    }
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    using (var deflateStream = new DeflateStream(responseStream, CompressionMode.Decompress))
                    {
                        using (var streamReader = new StreamReader(deflateStream, Encoding.Default))
                        {
                            html = streamReader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        html = reader.ReadToEnd();
                    }
                }
            }

            return html;
        }

        private static string FormatEndpoint(Endpoint endpoint)
        {
            return endpoint == Endpoint.Betting ? "betting" : "account";
        }
    }
}