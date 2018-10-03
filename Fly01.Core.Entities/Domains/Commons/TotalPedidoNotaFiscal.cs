using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [NotMapped]
    public class TotalPedidoNotaFiscal
    {                
        public double? TotalProdutos { get; set; }

        public double? TotalImpostosProdutos { get; set; }
                
        public double? TotalImpostosProdutosNaoAgrega { get; set; }

        public double? TotalServicos { get; set; }

        public double? TotalRetencoesServicos { get; set; }
        
        public double? TotalImpostosServicosNaoAgrega { get; set; }

        public double? ValorFrete { get; set; }

        public double Total
        {
            get
            {
                return
                    (
                    Math.Round(
                        (
                            (
                                (TotalProdutos.HasValue ? TotalProdutos.Value : 0) +
                                (TotalServicos.HasValue ? TotalServicos.Value : 0) +
                                (TotalImpostosProdutos.HasValue ? TotalImpostosProdutos.Value : 0) +
                                (ValorFrete.HasValue ? ValorFrete.Value : 0)
                            ) -
                            (TotalRetencoesServicos.HasValue ? TotalRetencoesServicos.Value : 0)
                        )
                    , 2, MidpointRounding.AwayFromZero)
                    
                );
            }
        }
    }
}