using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Exception = BetfairNG.Data.Exceptions.Exception;

namespace BetfairNG
{
    public class Network
    {
        private static object lockObj = new object();
        public static TraceSource TraceSource = new TraceSource("BetfairNG.Network");

        public Network() : this(null, null)
        {
        }

        public Network(
            string appKey,
            string sessionToken,
            Action preRequestAction = null,
            bool gzipCompress = true,
            WebProxy proxy = null)
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

        public string UserAgent { get; set; }
        public string Host { get; set; }
        public string AppKey { get; set; }
        public string SessionToken { get; set; }

        public int TimeoutMilliseconds { get; set; }
        public int RetryCount { get; set; }
        public bool GZipCompress { get; set; }
        public Action PreRequestAction { get; set; }
        public WebProxy Proxy { get; set; }

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
                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    var lastByte = DateTime.Now;
                    var response = JsonConvert.Deserialize<KeepAliveResponse>(reader.ReadToEnd());
                    watch.Stop();
                    TraceSource.TraceInformation("KeepAlive finish: {0}ms", watch.ElapsedMilliseconds);
                    var r = new BetfairServerResponse<KeepAliveResponse>();
                    r.HasError = !string.IsNullOrWhiteSpace(response.Error);
                    r.Response = response;
                    r.LastByte = lastByte;
                    r.RequestStart = requestStart;
                    return r;
                }
            }
        }

        public Task<BetfairServerResponse<T>> Invoke<T>(
            Exchange exchange,
            Endpoint endpoint,
            string method,
            IDictionary<string, object> args = null)
        {
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new ArgumentNullException("method");
            }

            TraceSource.TraceInformation("Network: {0}, {1}", this.FormatEndpoint(endpoint), method);
            var requestStart = DateTime.Now;
            var watch = new Stopwatch();
            watch.Start();

            var url = string.Empty;
            if (exchange == Exchange.AUS)
            {
                url = "https://api-au.betfair.com/exchange";
            }
            else
            {
                url = "https://api.betfair.com/exchange";
            }

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
                TraceSource.TraceInformation("Network finish: {0}ms, {1}, {2}",
                    watch.ElapsedMilliseconds,
                    this.FormatEndpoint(endpoint),
                    method);

                return this.ToResponse(jsonResponse, requestStart, lastByte, watch.ElapsedMilliseconds);
            });

            return result;
        }

        private BetfairServerResponse<T> ToResponse<T>(JsonResponse<T> response, DateTime requestStart, DateTime lastByteStamp, long latency)
        {
            var r = new BetfairServerResponse<T>();
            r.Error = BetfairServerException.ToClientException(response.Error);
            r.HasError = response.HasError;
            r.Response = response.Result;
            r.LastByte = lastByteStamp;
            r.RequestStart = requestStart;
            return r;
        }

        private Task<string> Request(
            string url,
            string requestPostData,
            string contentType,
            string appKey,
            string sessionToken)
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

                    return task.ContinueWith(t => this.GetResponseHtml((HttpWebResponse) t.Result));
                }).Unwrap();

            return continuation;
        }

        private string GetResponseHtml(HttpWebResponse response)
        {
            var html = string.Empty;

            using (var responseStream = response.GetResponseStream())
            {
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

        private string FormatEndpoint(Endpoint endpoint)
        {
            return endpoint == Endpoint.Betting ? "betting" : "account";
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class JsonRequest
        {
            public JsonRequest()
            {
                this.JsonRpc = "2.0";
            }

            [JsonProperty(PropertyName = "jsonrpc", NullValueHandling = NullValueHandling.Ignore)]
            public string JsonRpc { get; set; }

            [JsonProperty(PropertyName = "method")]
            public string Method { get; set; }

            [JsonProperty(PropertyName = "params")]
            public object Params { get; set; }

            [JsonProperty(PropertyName = "id")]
            public object Id { get; set; }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class JsonResponse<T>
        {
            [JsonProperty(PropertyName = "jsonrpc", NullValueHandling = NullValueHandling.Ignore)]
            public string JsonRpc { get; set; }

            [JsonProperty(PropertyName = "result", NullValueHandling = NullValueHandling.Ignore)]
            public T Result { get; set; }

            [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Ignore)]
            public Exception Error { get; set; }

            [JsonProperty(PropertyName = "id")]
            public object Id { get; set; }

            [JsonIgnore]
            public bool HasError
            {
                get { return this.Error != null; }
            }
        }
    }
}