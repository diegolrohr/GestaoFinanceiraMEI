﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFSeServico : NotaFiscalItem
    {
        [Required]
        public Guid ServicoId { get; set; }

        public double ValorOutrasRetencoes { get; set; }

        public string DescricaoOutrasRetencoes { get; set; }

        public bool IsServicoPrioritario { get; set; }

        public virtual Servico Servico { get; set; }

        [NotMapped]
        public Guid OrdemVendaServicoId { get; set; }
    }
}