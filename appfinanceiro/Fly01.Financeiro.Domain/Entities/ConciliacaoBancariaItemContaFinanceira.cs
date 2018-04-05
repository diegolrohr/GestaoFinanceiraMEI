using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System;
using Fly01.Core.Domain;

namespace Fly01.Financeiro.Domain.Entities
{
    public class ConciliacaoBancariaItemContaFinanceira : PlataformaBase
    {
        [Required]
        public Guid ConciliacaoBancariaItemId { get; set; }

        [Required]
        public Guid ContaFinanceiraId { get; set; }

        public Guid? ContaFinanceiraBaixaId { get; set; }

        [Required]
        public double ValorConciliado { get; set; }

        #region Navigations Properties

        public virtual ContaFinanceira ContaFinanceira { get; set; }
        public virtual ContaFinanceiraBaixa ContaFinanceiraBaixa { get; set; }

        #endregion
    }
}