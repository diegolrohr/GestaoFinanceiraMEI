using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFeImportada : PlataformaBase
    {
        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Xml { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Json { get; set; }

        [Required]
        [MaxLength(32)]
        public string XmlMd5 { get; set; }
    }
}