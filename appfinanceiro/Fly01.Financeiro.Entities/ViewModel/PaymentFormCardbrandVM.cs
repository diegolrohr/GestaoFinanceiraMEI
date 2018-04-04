using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class PaymentFormCardbrandVM : DomainBaseVM
    {
        //
        [JsonProperty("paymentFormId")]
        [Display(Name = "Codigo")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public Guid PaymentFormId { get; set; }

        //
        [JsonProperty("cardBrandId")]
        [Display(Name = "Bandeira De Cartão")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public Guid CardBrandId { get; set; }

        //
        [JsonProperty("cardBrandDescription")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public Guid CardBrandDescription { get; set; }

        //
        [JsonProperty("cardBrandType")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public Guid CardBrandType { get; set; }
    }
}
