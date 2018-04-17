using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.Domain.Entities
{
    public class Pessoa : PessoaBase
    {
        [JsonIgnore]
        public virtual Cidade Cidade { get; set; }

        [JsonIgnore]
        public virtual Estado Estado { get; set; }
    }
}