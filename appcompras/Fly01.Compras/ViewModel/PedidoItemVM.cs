using Newtonsoft.Json;
using System;

namespace Fly01.Compras.ViewModel
{
    public class PedidoItemVM : OrdemCompraItemVM
    {
        [JsonProperty("pedidoId")]
        public Guid PedidoId { get; set; }
        
        [JsonProperty("pedido")]
        public virtual PedidoVM Pedido { get; set; }
    }
}