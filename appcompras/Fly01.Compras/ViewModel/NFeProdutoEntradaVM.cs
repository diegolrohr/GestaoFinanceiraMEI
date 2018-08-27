using System;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Compras.ViewModel
{
    [Serializable]
    public class NFeProdutoEntradaVM : NotaFiscalItemEntradaVM
    {
        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("valorCreditoICMS")]
        public double? ValorCreditoICMS { get; set; }

        [JsonProperty("valorICMSSTRetido")]
        public double? ValorICMSSTRetido { get; set; }

        [JsonProperty("valorBCSTRetido")]
        public double? ValorBCSTRetido { get; set; }

        [JsonProperty("valorFCPSTRetidoAnterior")]
        public double ValorFCPSTRetidoAnterior { get; set; }

        [JsonProperty("valorBCFCPSTRetidoAnterior")]
        public double ValorBCFCPSTRetidoAnterior { get; set; }

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }
    }
}