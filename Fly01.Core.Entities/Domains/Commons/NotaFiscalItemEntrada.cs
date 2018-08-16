using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public abstract class NotaFiscalItemEntrada : PlataformaBase
    {
        [Required]
        public Guid NotaFiscalEntradaId { get; set; }

        [Required]
        public Guid GrupoTributarioId { get; set; }

        [Required]
        public double Quantidade { get; set; }

        [Required]
        public double Valor { get; set; }

        public double Desconto { get; set; }

        public double Total
        {
            get
            {
                return Math.Round(((Quantidade * Valor) - Desconto), 2, MidpointRounding.AwayFromZero);
            }
            set
            { }
        }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public virtual NotaFiscalEntrada NotaFiscalEntrada { get; set; }
        public virtual GrupoTributario GrupoTributario { get; set; }
    }
}