using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class CondicaoParcelamentoParcelaVM
    {
        [JsonProperty("descricaoParcela")]
        public string DescricaoParcela { get; set; }

        [JsonProperty("dataVencimento")]
        public DateTime DataVencimento { get; set; }

        [JsonProperty("dataVencimentoString")]
        public string DataVencimentoString { get { return DataVencimento.ToString("dd/MM/yyyy"); } }

        [JsonProperty("valor")]
        public double Valor { get; set; }
    }
}