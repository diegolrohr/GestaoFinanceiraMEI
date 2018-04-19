using System;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class SerieNotaFiscalInutilizadaVM : DomainBaseVM
    {
        [JsonProperty("serie")]
        public string Serie { get; set; }

        [JsonProperty("tipoOperacaoSerieNotaFiscal")]
        [APIEnum("TipoOperacaoSerieNotaFiscal")]
        public string TipoOperacaoSerieNotaFiscal { get; set; }

        [JsonProperty("numNotaFiscal")]
        public int NumNotaFiscal { get; set; }

        [JsonProperty("statusSerieNotaFiscal")]
        [APIEnum("StatusSerieNotaFiscal")]
        public string StatusSerieNotaFiscal { get; set; }
    }
}
