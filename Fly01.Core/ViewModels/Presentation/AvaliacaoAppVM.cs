using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation
{
    public class AvaliacaoAppVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("titulo")]
        public string Titulo { get; set; }

        [JsonProperty("menu")]
        public string Menu { get; set; }

        [JsonProperty("satisfacao")]
        public string Satisfacao { get; set; }

        [JsonProperty("aplicativo")]
        public string Aplicativo { get; set; }
    }
}
