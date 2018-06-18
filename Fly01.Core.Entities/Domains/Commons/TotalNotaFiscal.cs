using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [NotMapped]
    public class TotalNotaFiscal
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
        public double Total
        {
            get
            {
                return (Math.Round(
                    ((TotalProdutos.HasValue ? TotalProdutos.Value : 0) +
                    (TotalServicos.HasValue ? TotalServicos.Value : 0) +
                    (TotalImpostosProdutos.HasValue ? TotalImpostosProdutos.Value : 0) +
                    (TotalImpostosServicos.HasValue ? TotalImpostosServicos.Value : 0) +
                    (ValorFrete.HasValue ? ValorFrete.Value : 0)), 2, MidpointRounding.AwayFromZero)
                );
            }
        }
    }
}