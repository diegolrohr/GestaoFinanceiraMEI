using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("Cest")]
    public abstract class CestBase : DomainBase
    {
        [Required]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(650)]
        public string Descricao { get; set; }

        public string Segmento { get; set; }

        public string Item { get; set; }

        public string Anexo { get; set; }

        public Guid? NcmId { get; set; }
    }
}
