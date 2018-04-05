using Fly01.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Compras.Domain.Entities
{
    public abstract class OrdemCompraItem : PlataformaBase
    {
        [Required]
        public Guid ProdutoId { get; set; }

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
                return Math.Round(((Quantidade * Valor) - Desconto),2, MidpointRounding.AwayFromZero);
            }
            set
            {

            }
        }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        #region Navigations Properties

        public virtual Produto Produto { get; set; }

        #endregion
    }
}
