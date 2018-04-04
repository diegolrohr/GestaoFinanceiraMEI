using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class CollectionSituationVM : DomainBaseVM
    {
        //Descrição da situaçao de cobrança.
        [JsonProperty("description")]
        [Display(Name = "Descr.Sit.")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(25, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Description { get; set; }

        //Cobrança em bordero: S = Sim, N= Não
        [JsonProperty("situationBord")]
        [Display(Name = "Sit.Bordero")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string SituationBord { get; set; }

        //Cobrança descontada: S = Sim, N= Não
        [JsonProperty("discounted")]
        [Display(Name = "Descontada")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Discounted { get; set; }
    }
}
