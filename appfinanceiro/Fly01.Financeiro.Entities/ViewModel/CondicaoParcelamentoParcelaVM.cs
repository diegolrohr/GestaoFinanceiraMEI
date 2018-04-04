using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class ResponseSimulacaoVM
    {
        [JsonProperty("value")]
        public List<CondicaoParcelamentoParcelaVM> Items { get; set; }
    }

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