using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Cnab : PlataformaBase
    {
        [Required]
        public int NumeroBoleto { get; set; }

        [Required]
        public double ValorBoleto { get; set; }

        public Guid? SacadoId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public Guid BancoCedenteId { get; set; }

        [Required]
        public DateTime DataEmissao { get; set; }

        [Required]
        public DateTime DataVencimento { get; set; }

        [Required]
        public string NossoNumero { get; set; }

        public Guid? ContaReceberId { get; set; }

        public Guid? ContaBancariaCedenteId { get; set; }

        public virtual Banco BancoCedente { get; set; }
        public virtual ContaBancaria ContaBancariaCedente { get; set; }
        public virtual Pessoa Sacado { get; set; }
        public virtual ContaReceber ContaReceber { get; set; }
    }
}