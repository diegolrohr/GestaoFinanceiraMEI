﻿using Fly01.Financeiro.Domain.Enums;
using Fly01.Core.Entities.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.Domain.Entities
{
    public class ContaFinanceiraRenegociacao : PlataformaBase
    {
        [Required]
        public virtual List<Guid> ContasFinanceirasOrigemIds { get; set; }

        [Required]
        public Guid PessoaId { get; set; }

        [Required]
        public TipoContaFinanceira TipoContaFinanceira { get; set; }

        [Required]
        public double ValorAcumulado { get; set; }

        [Required]
        public TipoRenegociacaoValorDiferenca TipoRenegociacaoValorDiferenca { get; set; }

        [Required]
        public TipoRenegociacaoCalculo TipoRenegociacaoCalculo { get; set; }

        [Required]
        public double ValorDiferenca { get; set; }

        [Required]
        public double ValorFinal { get; set; }

        [Required]
        public Guid CategoriaId { get; set; }

        [Required]
        public Guid FormaPagamentoId { get; set; }

        [Required]
        public Guid CondicaoParcelamentoId { get; set; }

        [Required]
        [Column(TypeName="date")]
        public DateTime DataEmissao { get; set; }

        [Required]
        [Column(TypeName="date")]
        public DateTime DataVencimento { get; set; }

        [Required]
        public string Descricao { get; set; }

        public string Motivo { get; set; }

        #region Navigation Properties

        public virtual Categoria Categoria { get; set; }
        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }

        #endregion
    }
}