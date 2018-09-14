using Fly01.Core.Entities.Domains;
using System.ComponentModel.DataAnnotations;
using System;

namespace Fly01.EmissaoNFE.Domain
{
    public class Siafi : DomainBase
    {
        [Required]
        [StringLength(7)]
        public string CodigoIbge { get; set; }

        [Required]
        [StringLength(7)]
        public string CodigoSiafi { get; set; }
    }
}

