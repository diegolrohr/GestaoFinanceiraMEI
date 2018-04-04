using System;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class CashFlowVM : DomainBaseVM
    {
        [JsonProperty("paymentDate")]
        public string PaymentDate { get; set; }

        [JsonProperty("paymentMonth")]
        public string PaymentMonth { get; set; }
        
        [JsonProperty("payments")]
        public double Payments { get; set; }
        
        [JsonProperty("receipts")]
        public double Receipts { get; set; }

        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("IsInitialBalance")]
        public int IsInitialBalance { get; set; }

        [JsonIgnore]
        public double SaldoAtual{ get; set; }

        [JsonIgnore]
        public double TotalReceberHoje { get; set; }

        [JsonIgnore]
        public double TotalPagarHoje { get; set; }
    }
}