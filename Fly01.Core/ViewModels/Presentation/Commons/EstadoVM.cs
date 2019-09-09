using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class EstadoVM : EmpresaBaseVM
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
