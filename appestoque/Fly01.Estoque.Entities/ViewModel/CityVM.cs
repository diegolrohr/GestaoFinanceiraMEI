using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class CityVM
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        [Display(Name = "Código Ibge")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(7, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Code { get; set; }

        //Descrição do Municipío
        [JsonProperty("description")]
        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(35, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Description { get; set; }

        //Sigla do Estado
        [JsonProperty("state")]
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string State { get; set; }

        [JsonProperty("ibgeCityCode")]
        [Display(Name = "Código IBGE")]
        public string IbgeCityCode { get; set; }

    }
}