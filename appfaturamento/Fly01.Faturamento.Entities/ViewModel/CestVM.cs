using Fly01.Core.VM;
using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.Entities.ViewModel
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

        #region NavigationProperties

        [JsonProperty("ncm")]
        public virtual NCMVM Ncm { get; set; }

        #endregion
    }
}
