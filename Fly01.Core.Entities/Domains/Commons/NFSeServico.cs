using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFSeServico : NotaFiscalItem
    {
        [Required]
        public Guid ServicoId { get; set; }

        public double ValorOutrasRetencoes { get; set; }

        public string DescricaoOutrasRetencoes { get; set; }

        public virtual Servico Servico { get; set; }

        protected override double GetTotal()
        {
            return Quantidade > 0 ? Math.Round(((Quantidade * Valor) - Desconto - ValorOutrasRetencoes), 2, MidpointRounding.AwayFromZero) : Math.Round((Valor - Desconto), 2, MidpointRounding.AwayFromZero);
        }
    }
}