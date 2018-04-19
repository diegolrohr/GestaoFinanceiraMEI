using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Estoque.ViewModel
{
    [Serializable]
    public class TipoMovimentoVM : DomainBaseVM
    {
        public const int DescricaoMaxLength = 40;

        [JsonProperty("descricao")]
        [StringLength(DescricaoMaxLength, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Descricao { get; set; }

        [JsonProperty("tipoEntradaSaida")]
        [Display(Name = "Tipo Entrada Saída")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [APIEnum("TipoEntradaSaida")]
        public string TipoEntradaSaida { get; set; }
    }
}