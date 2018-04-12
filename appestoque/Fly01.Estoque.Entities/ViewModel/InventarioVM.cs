using Fly01.Core.Attribute;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class InventarioVM : DomainBaseVM
    {
        [JsonProperty("dataUltimaInteracao")]
        public DateTime DataUltimaInteracao { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("inventarioStatus")]
        [APIEnum("InventarioStatus")]
        public string InventarioStatus { get; set; }

        #region Navigation
        
        #endregion Navigation

    }
}
