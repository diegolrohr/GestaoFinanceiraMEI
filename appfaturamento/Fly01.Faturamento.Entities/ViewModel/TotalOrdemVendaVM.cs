using System;
using Newtonsoft.Json;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class TotalOrdemVendaVM
    {
        [JsonProperty("totalProdutos")]
        public double? TotalProdutos { get; set; }

        [JsonProperty("totalImpostosProdutos")]
        public double? TotalImpostosProdutos { get; set; }

        [JsonProperty("totalServicos")]
        public double? TotalServicos { get; set; }

        [JsonProperty("totalImpostosServicos")]
        public double? TotalImpostosServicos { get; set; }

        [JsonProperty("valorFreteCIF")]
        public double? ValorFreteCIF { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
}