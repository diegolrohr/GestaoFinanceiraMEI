using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class FeriadoVM : DomainBaseVM
    {
        //Informe o Dia do Feriado
        [JsonProperty("day")]
        [Display(Name = "Dia")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Dia { get; set; }

        //Informe o Mês do Feriado
        [JsonProperty("month")]
        [Display(Name = "Mes")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Mes { get; set; }

        //Informe o Ano do Feriado
        [JsonProperty("year")]
        [Display(Name = "Ano")]
        [StringLength(4, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Ano { get; set; }

        //Informe a Descrição do Feriado
        [JsonProperty("description")]
        [Display(Name = "Descricao")]
        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }
    }
}
