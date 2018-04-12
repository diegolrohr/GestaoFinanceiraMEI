using System;
using Fly01.Core.Entities.Domains;
using Fly01.Faturamento.Domain.Enums;

namespace Fly01.Faturamento.Domain.Entities
{
    public abstract class ContaFinanceira : PlataformaBase
    {
        public ContaFinanceira()
        {
            TipoContaFinanceira = GetTipoContaFinceira();
        }

        public Guid? ContaFinanceiraRepeticaoPaiId { get; set; }

        public double ValorPrevisto { get; set; }

        public double? ValorPago { get; set; }

        public Guid CategoriaId { get; set; }

        public Guid CondicaoParcelamentoId { get; set; }

        public Guid PessoaId { get; set; }

        public DateTime DataEmissao { get; set; }

        public DateTime DataVencimento { get; set; }

        public string Descricao { get; set; }

        public string Observacao { get; set; }

        public Guid FormaPagamentoId { get; set; }

        public TipoContaFinanceira TipoContaFinanceira { get; set; }

        public abstract TipoContaFinanceira GetTipoContaFinceira();

        public StatusContaBancaria StatusContaBancaria { get; set; }

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
