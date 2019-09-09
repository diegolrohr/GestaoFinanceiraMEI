using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class ContaFinanceiraBaixaVM : EmpresaBaseVM
    {
        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("contaFinanceiraId")]
        public Guid ContaFinanceiraId { get; set; }

        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("contaFinanceira")]
        public virtual ContaFinanceiraVM ContaFinanceira { get; set; }

        [JsonProperty("contaBancaria")]
        public virtual ContaBancariaVM ContaBancaria { get; set; }
    }
}