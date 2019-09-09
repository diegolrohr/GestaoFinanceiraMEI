using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Financeiro.ViewModel;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class ConciliacaoBancariaItemContaFinanceiraVM : EmpresaBaseVM
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("conciliacaoBancariaItemId")]
        public Guid ConciliacaoBancariaItemId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("contaFinanceiraId")]
        public Guid ContaFinanceiraId { get; set; }

        [JsonProperty("contaFinanceiraBaixaId")]
        public Guid? ContaFinanceiraBaixaId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("valorConciliado")]
        public double ValorConciliado { get; set; }

        [JsonProperty("contaFinanceira")]
        public virtual ContaFinanceiraVM ContaFinanceira { get; set; }
        [JsonProperty("contaFinanceiraBaixa")]
        public virtual ContaFinanceiraBaixaVM ContaFinanceiraBaixa { get; set; }
    }
}