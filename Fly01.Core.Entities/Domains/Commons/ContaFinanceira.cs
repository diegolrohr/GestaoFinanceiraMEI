﻿using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    /*
     * ATENÇÃO: Entidade utiliza StoreProcedures
     * para processamento do Numero.
     * MigrationName: ContaFinanceiraCreateSP
     * (ISrael)
    */
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
        [Column(TypeName = "date")]
        public DateTime DataEmissao { get; set; }

        [Required]
        [Column(TypeName = "date")]
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

        public double Saldo
        {
            get
            {
                return ValorPrevisto - (ValorPago.HasValue ? ValorPago.Value : 0.00);
            }
            set
            { }
        }

        public int Numero { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataDesconto { get; set; }

        public double? ValorDesconto { get; set; }
        public string NomePessoa { get; set; }

        public Guid ContaBancariaId { get; set; }

        public virtual ContaFinanceira ContaFinanceiraRepeticaoPai { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual ContaBancaria ContaBancaria { get; set; }
    }
}