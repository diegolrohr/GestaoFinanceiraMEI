using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [NotMapped]
    public class TotalOrdemServico
    {
        [JsonProperty("quantidadeItensCliente")]
        public double? QuantidadeItensCliente { get; set; }

        [JsonProperty("totalProdutos")]
        public double? TotalProdutos { get; set; }

        [JsonProperty("totalServicos")]
        public double? TotalServicos { get; set; }

        [JsonProperty("total")]
        public double Total
        {
            get
            {
                return (Math.Round(
                    ((TotalProdutos.HasValue ? TotalProdutos.Value : 0) +
                    (TotalServicos.HasValue ? TotalServicos.Value : 0)), 2, MidpointRounding.AwayFromZero)
                );
            }
        }
    }
}
