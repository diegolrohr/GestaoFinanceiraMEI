using System;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel.Base
{
    [Serializable]
    public class ContaFinanceiraCompensacaoBaseVM : DomainBaseVM
    {
        [JsonProperty("value")]
        public double ValorCompensar { get; set; }

        [JsonIgnore]
        public string IdContaRef { get; set; }

        [JsonIgnore]
        public DateTime Data { get; set; }

        [JsonIgnore]
        public double ValorConta { get; set; }

        [JsonIgnore]
        public double SaldoCredito { get; set; }

        [JsonIgnore]
        public string Person { get; set; }

        [JsonIgnore]
        public string PersonName { get; set; }
    }
}
