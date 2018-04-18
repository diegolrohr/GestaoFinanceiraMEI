using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System;
using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Commons;

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

        public virtual ContaFinanceira ContaFinanceira { get; set; }

        public virtual ContaFinanceiraBaixa ContaFinanceiraBaixa { get; set; }
    }
}