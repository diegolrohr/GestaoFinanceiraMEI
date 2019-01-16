using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class AgendaVM
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("className")]
        //[APIEnum("StatusOrdemServico")]
        public string ClassName { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }


        //[JsonProperty("oSId")]
        //public string OSId { get; set; }
    }
}
