using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class StoneTokenBaseVM
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
