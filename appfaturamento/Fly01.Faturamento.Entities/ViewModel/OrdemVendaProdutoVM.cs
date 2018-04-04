using System;
using Newtonsoft.Json;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class OrdemVendaProdutoVM : OrdemVendaItemVM
    {
        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("valorCreditoICMS")]
        public double? ValorCreditoICMS { get; set; }

        [JsonProperty("valorICMSSTRetido")]
        public double? ValorICMSSTRetido { get; set; }

        [JsonProperty("valorBCSTRetido")]
        public double? ValorBCSTRetido { get; set; }

        #region NavigationProperties

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }

        #endregion
    }
}