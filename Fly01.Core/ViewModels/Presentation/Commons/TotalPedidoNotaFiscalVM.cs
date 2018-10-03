using System;
using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class TotalPedidoNotaFiscalVM
    {
        [JsonProperty("totalProdutos")]
        public double? TotalProdutos { get; set; }

        [JsonProperty("totalImpostosProdutos")]
        public double? TotalImpostosProdutos { get; set; }

        [JsonProperty("totalImpostosProdutosNaoAgrega")]
        public double? TotalImpostosProdutosNaoAgrega { get; set; }

        [JsonProperty("totalServicos")]
        public double? TotalServicos { get; set; }

        [JsonProperty("totalRetencoesServicos")]
        public double? TotalRetencoesServicos { get; set; }

        [JsonProperty("totalImpostosServicosNaoAgrega")]
        public double? TotalImpostosServicosNaoAgrega { get; set; }

        [JsonProperty("valorFrete")]
        public double? ValorFrete { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
}