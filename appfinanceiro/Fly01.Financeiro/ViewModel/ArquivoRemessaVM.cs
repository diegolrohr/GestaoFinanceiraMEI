using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Financeiro.ViewModel
{
    public class ArquivoRemessaVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("totalBoletos")]
        public int TotalBoletos { get; set; }

        [JsonProperty("valorTotal")]
        public double ValorTotal { get; set; }

        [JsonProperty("statusArquivoRemessa")]
        [APIEnum("StatusArquivoRemessa")]
        public string StatusArquivoRemessa { get; set; }

        [JsonProperty("dataExportacao")]
        public DateTime DataExportacao { get; set; }

        [JsonProperty("dataRetorno")]
        public DateTime? DataRetorno { get; set; }
    }
}