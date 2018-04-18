using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class PedidoItem : OrdemCompraItem
    {
        [Required]
        public Guid PedidoId { get; set; }

        public virtual Pedido Pedido { get; set; }
    }
}
