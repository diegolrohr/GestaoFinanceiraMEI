using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Commons;
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

        public virtual ContaFinanceiraRenegociacao ContaFinanceiraRenegociacao { get; set; }

        public virtual ContaFinanceira ContaFinanceira { get; set; }
    }
}
