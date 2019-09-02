using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public abstract class RenegociacaoContaFinanceira : EmpresaBase
    {
        [Required]
        public Guid ContaFinanceiraRenegociacaoId { get; set; }

        [Required]
        public Guid ContaFinanceiraId { get; set; }

        public virtual ContaFinanceiraRenegociacao ContaFinanceiraRenegociacao { get; set; }

        public virtual ContaFinanceira ContaFinanceira { get; set; }
    }
}