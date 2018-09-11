using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public abstract class OrdemVendaItem : PlataformaBase
    {
        [Required]
        public Guid OrdemVendaId { get; set; }

        public Guid? GrupoTributarioId { get; set; }

        //[Required]
        public double Quantidade { get; set; }

        //[Required]
        public double Valor { get; set; }

        public double Desconto { get; set; }

        public double Total
        {
            get
            {
                return Quantidade > 0 ? Math.Round(((Quantidade * Valor) - Desconto), 2, MidpointRounding.AwayFromZero) : Math.Round((Valor - Desconto), 2, MidpointRounding.AwayFromZero);
            }
            set
            { }
        }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public virtual OrdemVenda OrdemVenda { get; set; }
        public virtual GrupoTributario GrupoTributario { get; set; }
    }
}