using Fly01.Core.Api;
using Fly01.Core.VM;
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
