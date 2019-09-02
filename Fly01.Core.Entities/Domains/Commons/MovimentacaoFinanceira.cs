using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("Movimentacao")]
    public class MovimentacaoFinanceira : EmpresaBase
    {
        [Required]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        [Required]
        public double Valor { get; set; }

        public Guid? ContaBancariaOrigemId { get; set; }

        public Guid? ContaBancariaDestinoId { get; set; }

        public Guid? ContaFinanceiraId { get; set; }

        public Guid? CategoriaId { get; set; }

        public string Descricao { get; set; }

        public virtual ContaBancaria ContaBancariaOrigem { get; set; }

        public virtual ContaBancaria ContaBancariaDestino { get; set; }

        public virtual ContaFinanceira ContaFinanceira { get; set; }
    }
}