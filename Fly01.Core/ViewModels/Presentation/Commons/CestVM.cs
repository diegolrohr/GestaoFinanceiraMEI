using System;
using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class CestVM : DomainBaseVM 
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("segmento")]
        public string Segmento { get; set; }

        [JsonProperty("item")]
        public string Item { get; set; }

        [JsonProperty("anexo")]
        public string Anexo { get; set; }

        [JsonProperty("ncmId")]
        public Guid? NcmId { get; set; }

        [JsonProperty("ncm")]
        public virtual NcmVM Ncm { get; set; }
    }
}