using System;
using Newtonsoft.Json;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;

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
    }
}