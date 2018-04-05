using Fly01.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Financeiro.Domain.Entities
{
    /*
     * ATENÇÃO: Entidade utiliza StoreProcedures
     * para recalculo de saldos.
     * (Follmann)
    */
    public class SaldoHistorico : PlataformaBase
    {
        [Required]
        [Column(TypeName="date")]
        public DateTime Data { get; set; }

        [Required]
        public Guid ContaBancariaId { get; set; }

        [Required]
        public double SaldoDia { get; set; }

        [Required]
        public double SaldoConsolidado { get; set; }

        [Required]
        public double TotalRecebimentos { get; set; }

        [Required]
        public double TotalPagamentos { get; set; }

        public virtual ContaBancaria ContaBancaria { get; set; }
    }
}