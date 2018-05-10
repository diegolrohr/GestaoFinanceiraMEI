using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class AvaliacaoApp : PlataformaBase
    {
        [Required]
        [StringLength(500, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("titulo")]
        public string Titulo { get; set; }

        [Required]
        [JsonProperty("menu")]
        public string Menu { get; set; }

        [Required]
        [JsonProperty("satisfacao")]
        public int Satisfacao { get; set; }

        [Required]
        [JsonProperty("aplicativo")]
        public string Aplicativo { get; set; }
    }
}
