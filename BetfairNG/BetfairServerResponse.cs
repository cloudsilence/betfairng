using System;

namespace BetfairNG
{
    public class BetfairServerResponse<T>
    {
        public T Response { get; set; }
        public DateTime LastByte { get; set; }
        public DateTime RequestStart { get; set; }
        public long LatencyMs { get; set; }
        public bool HasError { get; set; }
        public BetfairServerException Error { get; set; }
    }
}