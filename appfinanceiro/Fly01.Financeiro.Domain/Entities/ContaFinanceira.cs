﻿using Fly01.Financeiro.Domain.Enums;
using Fly01.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Financeiro.Domain.Entities
{
    public abstract class ContaFinanceira : PlataformaBase
    {
        public ContaFinanceira()
        {
            TipoContaFinanceira = GetTipoContaFinceira();
        }

        public Guid? ContaFinanceiraRepeticaoPaiId { get; set; }

        [Required]
        public double ValorPrevisto { get; set; }

        public double? ValorPago { get; set; }

        [Required]
        public Guid CategoriaId { get; set; }

        [Required]
        public Guid CondicaoParcelamentoId { get; set; }

        [Required]
        public Guid PessoaId { get; set; }

        [Required]
        [Column(TypeName="date")]
        public DateTime DataEmissao { get; set; }

        [Required]
        [Column(TypeName="date")]
        public DateTime DataVencimento { get; set; }

        [Required]
        public string Descricao { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        [Required]
        public Guid FormaPagamentoId { get; set; }

        [Required]
        public TipoContaFinanceira TipoContaFinanceira { get; set; }

        public abstract TipoContaFinanceira GetTipoContaFinceira();

        [Required]
        public StatusContaBancaria StatusContaBancaria { get; set; }

        [Required]
        public bool Repetir { get; set; }

        public TipoPeriodicidade TipoPeriodicidade { get; set; }

        public int? NumeroRepeticoes { get; set; }
        
        public string DescricaoParcela { get; set; }

        //AppDataContext model.builder ignore
        public double Saldo
        {
            get
            {
                return ValorPrevisto - (ValorPago.HasValue ? ValorPago.Value : 0.00);
            }
            set
            {
                 
            }
        }

        #region Navigation Properties

        public virtual ContaFinanceira ContaFinanceiraRepeticaoPai { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }

        #endregion
    }
}