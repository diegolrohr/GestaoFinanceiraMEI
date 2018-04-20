using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public abstract class NotaFiscalItem : PlataformaBase
    {
        [Required]
        public Guid NotaFiscalId { get; set; }

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
        }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public virtual NotaFiscal NotaFiscal { get; set; }
        public virtual GrupoTributario GrupoTributario { get; set; }
    }
}