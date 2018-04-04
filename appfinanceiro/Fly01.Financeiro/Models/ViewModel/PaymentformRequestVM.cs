using System;
using System.Collections.Generic;
using Fly01.Financeiro.Entities.ViewModel;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.ViewModel
{
    [Serializable]
    public class PaymentformRequestVM : FormaPagamentoVM
    {
        [JsonProperty("cardBrands")]
        public List<PaymentformCardBrandRequest> PaymentformCardBrandRequest { get; set; }

        [JsonProperty("paymentProviders")]
        public List<PaymentformProviderRequest> PaymentformProviderRequest { get; set; }

        [JsonProperty("items")]
        public List<PaymentformConditionRequest> PaymentformConditionRequest { get; set; }

        public PaymentformRequestVM()
        {
            PaymentformCardBrandRequest = new List<PaymentformCardBrandRequest>();
            PaymentformConditionRequest = new List<PaymentformConditionRequest>();
            PaymentformProviderRequest = new List<PaymentformProviderRequest>();
        }
    }

    #region Items
    [Serializable]
    public class PaymentformItemRequestBase
    {
        [JsonProperty("add")]
        public bool Add { get; set; }

        [JsonProperty("update")]
        public bool Update { get; set; }

        [JsonProperty("delete")]
        public bool Delete { get; set; }

        public PaymentformItemRequestBase()
        {
            Add = true;
            Update = true;
            Delete = false;
        }
    }

    [Serializable]
    public class PaymentformCardBrandRequest : PaymentformItemRequestBase
    {
        [JsonProperty("cardBrandId")]
        public string CardBrandId { get; set; }
    }

    [Serializable]
    public class PaymentformProviderRequest : PaymentformItemRequestBase
    {
        [JsonProperty("paymentProviderId")]
        public string PaymentProviderId { get; set; }
    }

    [Serializable]
    public class PaymentformConditionRequest : PaymentformItemRequestBase
    {
        [JsonProperty("conditionId")]
        public string ConditionId { get; set; }
    }
    #endregion    
}