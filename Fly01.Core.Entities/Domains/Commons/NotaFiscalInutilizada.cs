using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NotaFiscalInutilizada : PlataformaBase
    {
        [Required]
        [StringLength(3)]
        public string Serie { get; set; }

        [Required]
        public int NumNotaFiscal { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        [StringLength(44)]
        public string SefazChaveAcesso { get; set; }

        public StatusNotaFiscal Status { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Mensagem { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Recomendacao { get; set; }
    }
}