using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public abstract class UnidadeMedidaBaseVM : DomainBaseVM
    {
        [JsonProperty("abreviacao")]
        public string Abreviacao { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}
