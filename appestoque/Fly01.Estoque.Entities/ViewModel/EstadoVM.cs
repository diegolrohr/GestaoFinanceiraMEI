﻿using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.ViewModels.Commons;
using System;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class EstadoVM : DomainBaseVM
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

        [Required]
        [StringLength(2)]
        [Display(Name = "Código IBGE")]
        public string CodigoIBGE { get; set; }
    }
}