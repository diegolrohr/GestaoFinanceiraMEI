﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ConciliacaoBancaria : PlataformaBase
    {
        [Required]
        public Guid ContaBancariaId { get; set; }

        [NotMapped]
        public string Arquivo { get; set; }

        public virtual ContaBancaria ContaBancaria { get; set; }
        public virtual List<ConciliacaoBancariaItem> ConciliacaoBancariaItens { get; set; }
    }
}