using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class CategoriaVM : EmpresaBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("categoriaPaiId")]
        public Guid? CategoriaPaiId { get; set; }

        [JsonProperty("tipoCarteira")]
        [APIEnum("TipoCarteira")]
        public string TipoCarteira { get; set; }

        [JsonIgnore]
        public int Level => CategoriaPaiId == null ? 0 : 1;

        [JsonProperty("categoriaPai")]
        public CategoriaVM CategoriaPai { get; set; }
    }
}