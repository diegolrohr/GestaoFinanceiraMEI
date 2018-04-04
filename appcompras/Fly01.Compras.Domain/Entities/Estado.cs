using Fly01.Core.Api.Domain;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Compras.Domain.Entities
{
    public class Estado : DomainBase
    {
        [Required]
        [StringLength(2)]
        public string Sigla { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [Required]
        [StringLength(35)]
        public string UtcId { get; set; }

        [StringLength(2)]
        [Display(Name = "Código IBGE")]
        public string CodigoIbge { get; set; }
    }
}
