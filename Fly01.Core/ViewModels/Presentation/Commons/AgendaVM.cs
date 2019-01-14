using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class AgendaVM
    {
        [JsonProperty("entrega")]
        public DateTime Entrega { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("status")]
        [APIEnum("StatusOrdemServico")]
        public string Status { get; set; }

        [JsonProperty("oSId")]
        public string OSId { get; set; }
    }
}
