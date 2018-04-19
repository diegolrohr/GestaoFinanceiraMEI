using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Iss : DomainBase
    {
        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Descricao { get; set; }
    }
}
