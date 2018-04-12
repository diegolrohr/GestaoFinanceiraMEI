using Newtonsoft.Json;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class EstadoBaseVM : DomainBaseVM
    {
        [JsonProperty("sigla")]
        public string Sigla { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("utcId")]
        public string UtcId { get; set; }

        [JsonProperty("codigoIbge")]
        public string CodigoIbge { get; set; }
    }
}
