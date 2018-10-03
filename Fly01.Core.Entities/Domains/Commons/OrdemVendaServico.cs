using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class OrdemVendaServico : OrdemVendaItem
    {
        [Required]
        public Guid ServicoId { get; set; }

        public double? ValorOutrasRetencoes { get; set; }

        public string DescricaoOutrasRetencoes { get; set; }

        public virtual Servico Servico { get; set; }
    }
}