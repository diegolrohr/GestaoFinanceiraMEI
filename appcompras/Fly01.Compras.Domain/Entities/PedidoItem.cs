using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Compras.Domain.Entities
{
    public class PedidoItem : OrdemCompraItem
    {
        [Required]
        public Guid PedidoId { get; set; }

        #region Navigations Properties

        public virtual Pedido Pedido { get; set; }

        #endregion
    }
}
