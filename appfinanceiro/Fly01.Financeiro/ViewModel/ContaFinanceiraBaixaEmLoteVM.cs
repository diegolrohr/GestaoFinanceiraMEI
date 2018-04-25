using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class ContaFinanceiraBaixaEmLoteVM : DomainBaseVM
    {
        [Display(Name = "Data Baixa")]
        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [Display(Name = "Contas Financeiras")]
        [JsonProperty("contasFinanceirasIds")]
        public List<Guid> ContasFinanceirasIds { get; set; }

        [Display(Name = "Conta BancáriaId")]
        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }
        
        [Display(Name = "Observação")]
        [JsonProperty("observacao")]
        public string Observacao { get; set; }
        
        [JsonProperty("contaBancaria")]
        public virtual ContaBancariaVM ContaBancaria { get; set; }
    }
}