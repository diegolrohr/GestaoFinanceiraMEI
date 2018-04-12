using Fly01.Core.Entities.Domains;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Estoque.Domain.Entities
{
    [NotMapped]
    public class MovimentoOrdemVenda : PlataformaBase
    {
        public double QuantidadeBaixa { get; set; }
        public int PedidoNumero { get; set; }
        public Guid ProdutoId { get; set; }

        #region Navigations Properties

        public virtual Produto Produto { get; set; }

        #endregion
    }
}