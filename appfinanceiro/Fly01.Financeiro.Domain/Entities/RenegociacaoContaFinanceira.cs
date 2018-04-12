using Fly01.Core.Entities.Domains;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Financeiro.Domain.Entities
{
    public abstract class RenegociacaoContaFinanceira : PlataformaBase
    {
        [Required]
        public Guid ContaFinanceiraRenegociacaoId { get; set; }

        [Required]
        public Guid ContaFinanceiraId { get; set; }

        #region Navigations Properties

        public virtual ContaFinanceiraRenegociacao ContaFinanceiraRenegociacao { get; set; }
        public virtual ContaFinanceira ContaFinanceira { get; set; }

        #endregion
    }
}
