using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class PedidoItem : OrdemCompraItem
    {
        [Required]
        public Guid PedidoId { get; set; }

        public Guid? GrupoTributarioId { get; set; }

        public double ValorCreditoICMS { get; set; }

        public double ValorICMSSTRetido { get; set; }

        public double ValorBCSTRetido { get; set; }

        public double ValorFCPSTRetidoAnterior { get; set; }

        public double ValorBCFCPSTRetidoAnterior { get; set; }

        public double PercentualReducaoBC { get; set; }

        public double PercentualReducaoBCST { get; set; }

        public Guid? NFeImportacaoProdutoId { get; set; }

        public virtual GrupoTributario GrupoTributario { get; set; }
        public virtual Pedido Pedido { get; set; }
    }
}
