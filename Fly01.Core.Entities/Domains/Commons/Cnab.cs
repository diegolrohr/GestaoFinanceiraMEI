using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Cnab : PlataformaBase
    {
        [Required]
        public string Status { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataEmissao { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataVencimento { get; set; }

        [Required]
        public string NossoNumero { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataDesconto { get; set; }

        public double ValorDesconto { get; set; }

        public double ValorBoleto { get; set; }

        public Guid? ContaBancariaCedenteId { get; set; }

        public Guid? ContaReceberId { get; set; }

        public Guid? ArquivoRemessaId { get; set; }

        public virtual ContaBancaria ContaBancariaCedente { get; set; }
        public virtual ContaReceber ContaReceber { get; set; }
        public virtual ArquivoRemessa ArquivoRemessa { get; set; }
    }
}