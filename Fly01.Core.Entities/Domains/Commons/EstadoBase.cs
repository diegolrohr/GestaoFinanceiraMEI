using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("Estado")]
    public abstract class EstadoBase : DomainBase
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