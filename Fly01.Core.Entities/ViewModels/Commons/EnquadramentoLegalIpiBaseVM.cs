using Newtonsoft.Json;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class EnquadramentoLegalIpiBaseVM : DomainBaseVM
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("grupoCST")]
        public string GrupoCST { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}
