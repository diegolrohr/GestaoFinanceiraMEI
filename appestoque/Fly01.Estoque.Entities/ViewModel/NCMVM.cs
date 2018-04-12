using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class NCMVM : DomainBaseVM
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("aliquotaIPI")]
        public double AliquotaIPI { get; set; }
    }
}
