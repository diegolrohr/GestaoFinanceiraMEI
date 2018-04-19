using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
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
