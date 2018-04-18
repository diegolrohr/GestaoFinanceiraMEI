using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Cidade : DomainBase
    {
        [Required]
        [StringLength(35)]
        public string Nome { get; set; }

        [Required]
        [StringLength(7)]
        public string CodigoIbge { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public Guid EstadoId { get; set; }

        public virtual Estado Estado { get; set; }
    }
}
