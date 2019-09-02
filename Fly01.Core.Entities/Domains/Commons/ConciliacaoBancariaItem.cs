using Fly01.Core.Entities.Domains.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ConciliacaoBancariaItem : EmpresaBase
    {
        [Required]
        public Guid ConciliacaoBancariaId { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        [Required]
        [MaxLength(32)]
        public string OfxLancamentoMD5 { get; set; }

        [Required]
        public StatusConciliado StatusConciliado { get; set; }

        public virtual List<ConciliacaoBancariaItemContaFinanceira> ConciliacaoBancariaItemContasFinanceiras { get; set; }
    }
}