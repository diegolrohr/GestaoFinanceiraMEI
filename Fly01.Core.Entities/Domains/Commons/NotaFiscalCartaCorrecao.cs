using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NotaFiscalCartaCorrecao : PlataformaBase
    {
        public Guid NotaFiscalId { get; set; }

        public string IdRetorno { get; set; }

        public DateTime Data { get; set; }

        [Required]
        [StringLength(1000)]
        public string MensagemCorrecao { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Mensagem { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string XML { get; set; }

        public virtual NotaFiscal NotaFiscal { get; set; }
    }

}
