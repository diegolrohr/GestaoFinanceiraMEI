using System;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class NotaFiscalInutilizadaVM : DomainBaseVM
    {
        [JsonProperty("serie")]
        public string Serie { get; set; }

        [JsonProperty("numNotaFiscal")]
        public int NumNotaFiscal { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("sefazChaveAcesso")]
        public string SefazChaveAcesso { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }

        [JsonProperty("recomendacao")]
        public string Recomendacao { get; set; }
    }
}