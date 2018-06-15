using System;
using Newtonsoft.Json;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class TotalNotaFiscalVM
    {
        [JsonProperty("totalProdutos")]
        public double? TotalProdutos { get; set; }

        [JsonProperty("totalImpostosProdutos")]
        public double? TotalImpostosProdutos { get; set; }

        [JsonProperty("totalImpostosProdutosNaoAgrega")]
        public double? TotalImpostosProdutosNaoAgrega { get; set; }

        [JsonProperty("totalServicos")]
        public double? TotalServicos { get; set; }

        [JsonProperty("totalImpostosServicos")]
        public double? TotalImpostosServicos { get; set; }

        [JsonProperty("valorFrete")]
        public double? ValorFrete { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
}