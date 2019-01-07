using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class KitVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

    }
}