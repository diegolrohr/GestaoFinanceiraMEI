using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Compras.ViewModel
{
    [Serializable]
    public abstract class OrdemCompraItemVM : DomainBaseVM
    {
        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("desconto")]
        public double Desconto { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("valorCreditoICMS")]
        public double ValorCreditoICMS { get; set; }

        [JsonProperty("valorICMSSTRetido")]
        public double ValorICMSSTRetido { get; set; }

        [JsonProperty("valorBCSTRetido")]
        public double ValorBCSTRetido { get; set; }

        [JsonProperty("valorFCPSTRetidoAnterior")]
        public double ValorFCPSTRetidoAnterior { get; set; }

        [JsonProperty("valorBCFCPSTRetidoAnterior")]
        public double ValorBCFCPSTRetidoAnterior { get; set; }

        [JsonProperty("grupoTributarioId")]
        public Guid GrupoTributarioId { get; set; }

        [JsonProperty("grupoTributario")]
        public virtual GrupoTributarioVM GrupoTributario { get; set; }

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }
    }
}