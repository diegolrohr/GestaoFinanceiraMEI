using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("CondicaoParcelamento")]
    public abstract class CondicaoParcelamentoBase : PlataformaBase
    {
        [Required]
        [JsonProperty("descricao")]
        [Display(Name = "Descricao")]
        [StringLength(200, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }

        [Display(Name = "Quantidade de Parcelas")]
        [JsonProperty("qtdParcelas")]
        public int? QtdParcelas { get; set; }

        [Display(Name = "Condições de Parcelamento")]
        [JsonProperty("condicoesParcelamento")]
        public string CondicoesParcelamento { get; set; }
    }
}
