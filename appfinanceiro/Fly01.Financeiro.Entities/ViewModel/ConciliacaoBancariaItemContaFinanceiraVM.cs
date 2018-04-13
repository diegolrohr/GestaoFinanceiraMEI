using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Financeiro.Entities.ViewModel.Base;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class ConciliacaoBancariaItemContaFinanceiraVM : DomainBaseVM
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

        #region Navigations Properties
        [JsonProperty("contaFinanceira")]
        public virtual ContaFinanceiraVM ContaFinanceira { get; set; }
        [JsonProperty("contaFinanceiraBaixa")]
        public virtual ContaFinanceiraBaixaVM ContaFinanceiraBaixa { get; set; }

        #endregion
    }
}