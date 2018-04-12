﻿using Fly01.Core.Entities.Domains;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Compras.Domain.Entities
{
    public class UnidadeMedida : DomainBase
    {
        [Required]
        [MaxLength(2)]
        public string Abreviacao { get; set; }

        [Required]
        [MaxLength(20)]
        public string Descricao { get; set; }
    }
}
