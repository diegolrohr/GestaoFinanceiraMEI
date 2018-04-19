using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public abstract class CfopBaseVM : DomainBaseVM
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}
