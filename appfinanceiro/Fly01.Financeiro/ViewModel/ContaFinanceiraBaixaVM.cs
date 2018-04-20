using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class ContaFinanceiraBaixaVM : DomainBaseVM
    {
        [Display(Name = "Data Baixa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("contaFinanceiraId")]
        [Display(Name = "Conta FinanceiraId")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid ContaFinanceiraId { get; set; }

        [JsonProperty("contaBancariaId")]
        [Display(Name = "Conta BancáriaId")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public double Valor { get; set; }

        [Display(Name = "Observação")]
        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("contaFinanceira")]
        public virtual ContaFinanceiraVM ContaFinanceira { get; set; }

        [JsonProperty("contaBancaria")]
        public virtual ContaBancariaVM ContaBancaria { get; set; }
    }
}