using Fly01.Core.Entities.Domains;
using Newtonsoft.Json;

namespace Fly01.Compras.Domain.Entities
{
    public class Categoria : CategoriaBase
    {
        [JsonIgnore]
        public virtual Categoria CategoriaPai { get; set; }
    }
}