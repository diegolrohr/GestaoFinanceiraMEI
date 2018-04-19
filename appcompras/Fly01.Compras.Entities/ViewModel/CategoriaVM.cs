using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Compras.Entities.ViewModel
{
    public class CategoriaVM : CategoriaBaseVM
    {
        [JsonProperty("categoriaPai")]
        public CategoriaVM CategoriaPai { get; set; }
    }
}