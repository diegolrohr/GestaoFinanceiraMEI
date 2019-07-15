using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFeProduto : NotaFiscalItem
    {
        [Required]
        public Guid ProdutoId { get; set; }

        public double ValorCreditoICMS { get; set; }

        public double ValorICMSSTRetido { get; set; }

        public double ValorBCSTRetido { get; set; }

        public double ValorFCPSTRetidoAnterior { get; set; }

        public double ValorBCFCPSTRetidoAnterior { get; set; }

        public double PercentualReducaoBC { get; set; }

        public double PercentualReducaoBCST { get; set; }

        [NotMapped]
        public Guid OrdemVendaProdutoId { get; set; }

        public virtual Produto Produto { get; set; }
    }
}