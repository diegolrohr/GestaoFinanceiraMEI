using Fly01.Core.Notifications;
using Newtonsoft.Json;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class AvaliacaoApp : RecordBaseMongoDB
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("titulo")]
        public string Titulo { get; set; }

        [JsonProperty("menu")]
        public string Menu { get; set; }

        [JsonProperty("satisfacao")]
        public int Satisfacao { get; set; }

        [JsonProperty("aplicativo")]
        public string Aplicativo { get; set; }
    }
}
