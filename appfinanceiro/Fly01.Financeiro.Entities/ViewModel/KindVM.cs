using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class KindVM : DomainBaseVM
    {
        //
        [JsonProperty("description")]
        [Display(Name = "Descricao")]
        [StringLength(150, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Description { get; set; }
    }
}
