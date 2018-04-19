using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Cfop : DomainBase
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        [StringLength(400)]
        public string Descricao { get; set; }

        [Required]
        public TipoCfop Tipo { get; set; }
    }
}
