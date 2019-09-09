using System;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Financeiro.ViewModel
{
    public class ReceitaPorCategoriaVM : EmpresaBaseVM
    {
        [JsonProperty("categoriaId")]
        public Guid CategoriaId { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("categoriaPaiId")]
        public Guid? CategoriaPaiId { get; set; }

        [JsonProperty("previsto")]
        public double Previsto { get; set; }

        [JsonProperty("realizado")]
        public double? Realizado { get; set; }

        [JsonProperty("soma")]
        public double Soma { get; set; }

        [JsonProperty("tipoCarteira")]
        [APIEnum("TipoCarteira")]
        public string TipoCarteira { get; set; }
    }
}