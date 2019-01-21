using Newtonsoft.Json;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ResponseAutenticacaoStone
    {
        [JsonProperty("token")]
        public string Token { get; set; }

    }
}
