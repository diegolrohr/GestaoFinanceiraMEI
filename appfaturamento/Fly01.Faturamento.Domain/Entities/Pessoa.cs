using Fly01.Core.Entities.Domains.Commons;
using Newtonsoft.Json;

namespace Fly01.Faturamento.Domain.Entities
{
    public class Pessoa : PessoaBase
    {
        [JsonIgnore]
        public virtual Cidade Cidade { get; set; }

        [JsonIgnore]
        public virtual Estado Estado { get; set; }
    }
}