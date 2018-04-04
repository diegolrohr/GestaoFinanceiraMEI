using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class PaymentProviderVM : DomainBaseVM
    {
        //
        [JsonProperty("description")]
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(64, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Description { get; set; }

        [JsonIgnore]
        public bool Selected { get; set; }
    }
}
