using Newtonsoft.Json;

namespace Fly01.Core.Api
{
    public class RequestBodyBasicAuth
    {
        [JsonProperty("authentication")]
        public AuthRequestBody Authentication { get; set; }
    }

    public class AuthRequestBody
    {
        public AuthRequestBody()
        {
            Type = "Basic";
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}