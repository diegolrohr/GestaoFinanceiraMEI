using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Domain;

namespace Fly01.Financeiro.Domain.Entities
{
    public class Banco : DomainBase
    {
        [Required]
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [Required]
        [JsonProperty("nome")]
        public string Nome { get; set; }
    }
}