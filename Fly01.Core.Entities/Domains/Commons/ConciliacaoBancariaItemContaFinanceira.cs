using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ConciliacaoBancariaItemContaFinanceira : EmpresaBase
    {
        [Required]
        public Guid ConciliacaoBancariaItemId { get; set; }

        [Required]
        public Guid ContaFinanceiraId { get; set; }

        public Guid? ContaFinanceiraBaixaId { get; set; }

        [Required]
        public double ValorConciliado { get; set; }

        public virtual ContaFinanceira ContaFinanceira { get; set; }

        public virtual ContaFinanceiraBaixa ContaFinanceiraBaixa { get; set; }
    }
}