using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Compras.ViewModel
{
    public class PedidoItemVM : OrdemCompraItemVM
    {
        [JsonProperty("pedidoId")]
        public Guid PedidoId { get; set; }

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

        [JsonProperty("percentualReducaoBC")]
        public double PercentualReducaoBC { get; set; }

        [JsonProperty("percentualReducaoBCST")]
        public double PercentualReducaoBCST { get; set; }

        [JsonProperty("grupoTributarioId")]
        public Guid? GrupoTributarioId { get; set; }

        [JsonProperty("grupoTributario")]
        public virtual GrupoTributarioVM GrupoTributario { get; set; }

        [JsonProperty("pedido")]
        public virtual PedidoVM Pedido { get; set; }
    }
}