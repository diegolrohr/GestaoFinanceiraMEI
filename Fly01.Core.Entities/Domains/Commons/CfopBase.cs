using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("Cfop")]
    public abstract class CfopBase : DomainBase
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
