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

        public Guid? PessoaId { get; set; }

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

        public Guid? ContaFinanceiraId { get; set; }

        public Guid? ContaBancariaSacadoId { get; set; }

        public virtual Pessoa Pessoa { get; set; }
        public virtual Banco BancoCedente { get; set; }
        public virtual ContaFinanceira ContaFinanceira { get; set; }
        public virtual ContaBancaria ContaBancariaSacado { get; set; }
    }
}