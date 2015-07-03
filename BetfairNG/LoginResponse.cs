using Newtonsoft.Json;

namespace BetfairNG
{
    public class LoginResponse
    {
        [JsonProperty(PropertyName = "sessionToken")]
        public string SessionToken { get; set; }

        [JsonProperty(PropertyName = "loginStatus")]
        public string LoginStatus { get; set; }
    }
}