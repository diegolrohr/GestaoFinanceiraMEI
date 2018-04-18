using System;
using Fly01.Core.Entities.Domains.Commons;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Faturamento.Domain.Entities
{
    public class OrdemVendaProduto : OrdemVendaItem
    {
        [Required]
        public Guid ProdutoId { get; set; }

        public double ValorCreditoICMS { get; set; }

        public double ValorICMSSTRetido { get; set; }

        public double ValorBCSTRetido { get; set; }

        public double ValorFCPSTRetidoAnterior { get; set; }

        public double AliquotaFCPConsumidorFinal { get; set; }

        #region Navigations Properties

        public virtual Produto Produto { get; set; }

        #endregion
    }
}
