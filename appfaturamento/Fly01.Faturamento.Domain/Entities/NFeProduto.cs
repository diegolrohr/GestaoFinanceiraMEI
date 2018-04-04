using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Faturamento.Domain.Entities
{
    public class NFeProduto : NotaFiscalItem
    {
        [Required]
        public Guid ProdutoId { get; set; }

        public double? ValorCreditoICMS { get; set; }

        public double? ValorICMSSTRetido { get; set; }

        public double? ValorBCSTRetido { get; set; }

        #region Navigations Properties

        public virtual Produto Produto { get; set; }

        #endregion
    }
}
