using System;
using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NotaFiscal : PlataformaBase
    {
        [StringLength(60)]
        public string NumeracaoVolumesTrans { get; set; }

        [StringLength(60)]
        public string Marca { get; set; }

        public Guid? OrdemVendaOrigemId { get; set; }

        [Required]
        public TipoNotaFiscal TipoNotaFiscal { get; set; }

        [Required]
        public TipoCompraVenda TipoVenda { get; set; }

        [Required]
        public StatusNotaFiscal Status { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        [Required]
        public Guid ClienteId { get; set; }

        #region Transporte

        public Guid? TransportadoraId { get; set; }

        public TipoFrete TipoFrete { get; set; }

        [StringLength(60)]
        public string TipoEspecie{ get; set; }

        [StringLength(7)]
        public string PlacaVeiculo { get; set; }

        public Guid? EstadoPlacaVeiculoId { get; set; }

        public string EstadoCodigoIbge { get; set; }

        public double? ValorFrete { get; set; }

        public double? PesoBruto { get; set; }

        public double? PesoLiquido { get; set; }

        public int? QuantidadeVolumes { get; set; }

        #endregion

        #region Pagamento

        public Guid? FormaPagamentoId { get; set; }

        public Guid? CondicaoParcelamentoId { get; set; }

        public Guid? CategoriaId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataVencimento { get; set; }

        #endregion

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public Guid? SerieNotaFiscalId { get; set; }

        public int? NumNotaFiscal { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string XML { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string PDF { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Mensagem { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Recomendacao { get; set; }

        [StringLength(60)]
        public string NaturezaOperacao { get; set; }

        /// <summary>
        /// Em NFe é a chave do sefaz gerada na transmissao
        /// Em NFSe é a concatenação da serie + número
        /// ambas utilizadas para o monitor dos status
        /// </summary>
        [StringLength(44)]
        public string SefazId { get; set; }

        [StringLength(44)]
        public string ChaveNFeReferenciada { get; set; }

        [Required]
        public bool NFeRefComplementarIsDevolucao { get; set; }

        [MaxLength(5000)]
        public string MensagemPadraoNota { get; set; }

        [MaxLength(1000)]
        public string InformacoesCompletamentaresNFS { get; set; }

        public bool GeraFinanceiro { get; set; }

        public Guid? ContaFinanceiraParcelaPaiIdProdutos { get; set; }

        public Guid? ContaFinanceiraParcelaPaiIdServicos { get; set; }

        public Guid? CertificadoDigitalId { get; set; }

        public TipoAmbiente TipoAmbiente { get; set; }

        public virtual OrdemVenda OrdemVendaOrigem { get; set; }
        public virtual Pessoa Cliente { get; set; }
        public virtual Pessoa Transportadora { get; set; }
        public virtual Estado EstadoPlacaVeiculo { get; set; }
        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual SerieNotaFiscal SerieNotaFiscal { get; set; }
        public virtual CertificadoDigital CertificadoDigital { get; set; }
    }
}