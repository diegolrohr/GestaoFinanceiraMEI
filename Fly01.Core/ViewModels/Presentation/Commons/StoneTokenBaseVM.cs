using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class StoneTokenBaseVM
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
