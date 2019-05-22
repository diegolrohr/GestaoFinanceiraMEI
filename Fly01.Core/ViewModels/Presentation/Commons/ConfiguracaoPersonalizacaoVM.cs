using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ConfiguracaoPersonalizacaoVM : DomainBaseVM
    {
        [JsonProperty("emiteNotaFiscal")]
        public bool EmiteNotaFiscal { get; set; }

        [JsonProperty("exibirStepProdutosVendas")]
        public bool ExibirStepProdutosVendas { get; set; }

        [JsonProperty("exibirStepProdutosCompras")]
        public bool ExibirStepProdutosCompras { get; set; }

        [JsonProperty("exibirStepServicosVendas")]
        public bool ExibirStepServicosVendas { get; set; }

        [JsonProperty("exibirStepServicosCompras")]
        public bool ExibirStepServicosCompras { get; set; }

        [JsonProperty("exibirStepTransportadoraVendas")]
        public bool ExibirStepTransportadoraVendas { get; set; }

        [JsonProperty("exibirStepTransportadoraCompras")]
        public bool ExibirStepTransportadoraCompras { get; set; }

    }
}