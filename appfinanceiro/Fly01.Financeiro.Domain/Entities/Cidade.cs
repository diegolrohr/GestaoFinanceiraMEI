using Fly01.Core.Api.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Financeiro.Domain.Entities
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

        #region NavigationProperties

        public virtual Estado Estado { get; set; }
        
        #endregion
    }
}
