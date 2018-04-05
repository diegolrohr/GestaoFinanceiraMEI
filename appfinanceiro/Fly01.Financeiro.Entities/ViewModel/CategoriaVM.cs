using System;
using Fly01.Core.VM;
using Newtonsoft.Json;
using Fly01.Core.Attribute;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class CategoriaVM : DomainBaseVM
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

        #region Navigations Properties

        [JsonProperty("categoriaPai")]
        public virtual CategoriaVM CategoriaPai { get; set; }

        #endregion
    }
}