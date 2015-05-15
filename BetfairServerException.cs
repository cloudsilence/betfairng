using System;
using Newtonsoft.Json.Linq;

namespace BetfairNG
{
    public class BetfairServerException : Exception
    {
        public JObject ServerData { get; set; }
        public JObject ServerDetail { get; set; }

        public static BetfairServerException ToClientException(Data.Exceptions.Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            var exception = new BetfairServerException { ServerData = ex.Data, ServerDetail = ex.Detail };
            return exception;
        }
    }
}