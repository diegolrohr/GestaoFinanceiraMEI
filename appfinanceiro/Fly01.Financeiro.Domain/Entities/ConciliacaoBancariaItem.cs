using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Fly01.Core.Entities.Domains;
using Fly01.Financeiro.Domain.Enums;

namespace Fly01.Financeiro.Domain.Entities
{
    public class ConciliacaoBancariaItem : PlataformaBase
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

        #region Navigations Properties

        public virtual List<ConciliacaoBancariaItemContaFinanceira> ConciliacaoBancariaItemContasFinanceiras { get; set; }

        #endregion
    }
}