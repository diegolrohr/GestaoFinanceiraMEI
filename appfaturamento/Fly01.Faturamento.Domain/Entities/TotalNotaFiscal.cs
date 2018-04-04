using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Faturamento.Domain.Entities
{
    [NotMapped]
    public class TotalNotaFiscal
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
        public double Total
        {
            get
            {
                return (Math.Round(
                    ((TotalProdutos.HasValue ? TotalProdutos.Value : 0) +
                    (TotalServicos.HasValue ? TotalServicos.Value : 0) +
                    (TotalImpostosProdutos.HasValue ? TotalImpostosProdutos.Value : 0) +
                    (TotalImpostosServicos.HasValue ? TotalImpostosServicos.Value : 0) +
                    (ValorFreteCIF.HasValue ? ValorFreteCIF.Value : 0)), 2, MidpointRounding.AwayFromZero)
                );
            }
            set { }
        }
    }
}