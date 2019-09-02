using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains
{
    public class EmpresaBase : DomainBase
    {
        [Required]
        public Guid EmpresaId { get; set; }
    }
}