using Fly01.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Faturamento.Domain.Entities
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

        //AppDataContext model.builder ignore
        public double Total
        {
            get
            {
                return Math.Round(((Quantidade * Valor) - Desconto), 2, MidpointRounding.AwayFromZero);
            }
            set
            {

            }
        }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        #region Navigations Properties

        public virtual NotaFiscal NotaFiscal { get; set; }
        public virtual GrupoTributario GrupoTributario { get; set; }

        #endregion
    }
}
