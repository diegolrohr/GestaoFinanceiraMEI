using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Treinamento : PlataformaBase
    {
        [Required]
        [StringLength(200, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }

        public int Numero { get; set; }
    }
}
