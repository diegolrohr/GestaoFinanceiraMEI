using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class ArquivoRemessaVM : DomainBaseVM
    {
        [JsonProperty("numeroArquivo")]
        public int NumeroArquivo { get; set; }

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
        public DateTime DataRetorno { get; set; }
    }
}