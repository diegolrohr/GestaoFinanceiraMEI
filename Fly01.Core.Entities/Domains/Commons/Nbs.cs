using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Nbs : DomainBase
    {
        [Required]
        public string Codigo { get; set; }

        [Required]
        [StringLength(600)]
        public string Descricao { get; set; }
    }
}