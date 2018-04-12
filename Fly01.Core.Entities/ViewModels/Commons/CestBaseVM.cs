using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class CestBaseVM : DomainBaseVM
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

        #region NavigationProperties
        [JsonProperty("ncm")]
        public virtual NcmBaseVM Ncm { get; set; }
        #endregion
    }
}
