using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class InventarioItemVM : DomainBaseVM
    {

        //Saldo atual do produto
        [JsonProperty("saldoInventariado")]
        public double SaldoInventariado { get; set; }

        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("inventarioId")]
        public Guid InventarioId { get; set; }

        #region Navigations Properties

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }

        [JsonProperty("inventario")]
        public virtual InventarioVM Inventario { get; set; }
        #endregion
    }
}
