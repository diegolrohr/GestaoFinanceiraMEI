using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public abstract class OrdemVendaItem : PlataformaBase
    {
        [Required]
        public Guid OrdemVendaId { get; set; }

        [Required]
        public Guid GrupoTributarioId { get; set; }

        [Required]
        public double Quantidade { get; set; }

        [Required]
        public double Valor { get; set; }

        public double Desconto { get; set; }

        [NotMapped]
        public double Total
        {
            get
            {
                return Math.Round(((Quantidade * Valor) - Desconto), 2, MidpointRounding.AwayFromZero);
            }
        }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public virtual OrdemVenda OrdemVenda { get; set; }
        public virtual GrupoTributario GrupoTributario { get; set; }
    }
}