using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class PaymentFormItemVM : DomainBaseVM
    {
        //
        [JsonProperty("paymentFormId")]
        [Display(Name = "Codigo")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string PaymentFormId { get; set; }

        //
        [JsonProperty("conditionId")]
        [Display(Name = "Cond. De Pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ConditionId { get; set; }

        //Informe a descrição da condição de pagamento
        [JsonProperty("conditionDescription")]
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ConditionDescription { get; set; }

        //
        [JsonProperty("minimumValue")]
        [Display(Name = "Valor Minimo")]
        public double? MinimumValue { get; set; }

        //
        [JsonProperty("cardFees")]
        [Display(Name = "Taxa Administrativa")]
        public double? CardFees { get; set; }

        //
        [JsonProperty("interest")]
        [Display(Name = "Juros")]
        public double? Interest { get; set; }
    }
}
