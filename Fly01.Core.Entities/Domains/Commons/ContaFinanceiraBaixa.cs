using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ContaFinanceiraBaixa : EmpresaBase
    {
        [Required]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        [Required]
        public Guid ContaFinanceiraId { get; set; }

        [Required]
        public Guid ContaBancariaId { get; set; }

        [Required]
        public double Valor { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public virtual ContaFinanceira ContaFinanceira { get; set; }

        public virtual ContaBancaria ContaBancaria { get; set; }
    }
}