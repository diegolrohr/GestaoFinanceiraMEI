using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("Movimento")]
    public class MovimentoEstoque : PlataformaBase
    {
        public double? QuantidadeMovimento { get; set; }

        public string Observacao { get; set; }
        public double? SaldoAntesMovimento { get; set; } 
        public Guid? TipoMovimentoId { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid? InventarioId { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Inventario Inventario { get; set; }
        public virtual TipoMovimento TipoMovimento { get; set; }
    }
}