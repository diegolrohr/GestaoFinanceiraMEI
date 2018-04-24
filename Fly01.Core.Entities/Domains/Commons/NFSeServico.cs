﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFSeServico : NotaFiscalItem
    {
        [Required]
        public Guid ServicoId { get; set; }

        public virtual Servico Servico { get; set; }
    }
}