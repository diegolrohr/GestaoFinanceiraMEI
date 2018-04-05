using Fly01.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Estoque.Domain.Entities
{
    public class Movimento : PlataformaBase
    {
        public double? QuantidadeMovimento { get; set; }
        [StringLength(200, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Observacao { get; set; }
        public double? SaldoAntesMovimento { get; set; } 
        public Guid? TipoMovimentoId { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid? InventarioId { get; set; }

        #region Navigations Properties

        public virtual Produto Produto { get; set; }
        public virtual Inventario Inventario { get; set; }
        public virtual TipoMovimento TipoMovimento { get; set; }

        #endregion
    }
}