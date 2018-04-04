using Newtonsoft.Json;
using System;

namespace Fly01.Compras.Entities.ViewModel
{
    public class PedidoItemVM : OrdemCompraItemVM
    {
        [JsonProperty("pedidoId")]
        public Guid PedidoId { get; set; }
        
        #region Navigations Properties

        [JsonProperty("pedido")]
        public virtual PedidoVM Pedido { get; set; }

        #endregion
    }
}
