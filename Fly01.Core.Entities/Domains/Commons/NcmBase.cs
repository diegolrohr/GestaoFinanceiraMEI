using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("Ncm")]
    public abstract class NcmBase : DomainBase
    {
        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Descricao { get; set; }

        public double AliquotaIPI { get; set; }
    }
}