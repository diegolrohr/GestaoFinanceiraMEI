using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class PaymentFormProviderVM : DomainBaseVM
    {
        //
        [JsonProperty("paymentFormId")]
        [Display(Name = "Codigo")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string PaymentFormId { get; set; }

        //
        [JsonProperty("paymentProviderId")]
        [Display(Name = "Operadora De Pagamento")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string PaymentProviderId { get; set; }

        //
        [JsonProperty("paymentProviderDescription")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string PaymentProviderDescription { get; set; }
    }
}
