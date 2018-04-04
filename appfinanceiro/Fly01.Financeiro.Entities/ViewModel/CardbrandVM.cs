using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class CardbrandVM : DomainBaseVM
    {
        //
        [JsonProperty("name")]
        [Display(Name = "Nome")]
        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Name { get; set; }

        //
        [JsonProperty("type")]
        [Display(Name = "Gera Cnab")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Type { get; set; }

        [JsonIgnore]
        public bool Selected { get; set; }
    }
}
