using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class CategoriaVM : CategoriaBaseVM
    {
        [JsonProperty("categoriaPai")]
        public CategoriaVM CategoriaPai { get; set; }
    }
}