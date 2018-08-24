using Newtonsoft.Json;
using System;

namespace Fly01.OrdemServico.ViewModel
{
    [Serializable]
    public class TotalOrdemServicoVM
    {
        [JsonProperty("quantidadeItensCliente")]
        public double? QuantidadeItensCliente { get; set; }

        [JsonProperty("totalProdutos")]
        public double? TotalProdutos { get; set; }

        [JsonProperty("totalServicos")]
        public double? TotalServicos { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

    }
}