using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class OrdemVenda : PlataformaBase
    {
        [Required]
        public int Numero { get; set; }

        [StringLength(60)]
        public string NumeracaoVolumesTrans { get; set; }

        [StringLength(60)]
        public string Marca { get; set; }

        [StringLength(44)]
        public string ChaveNFeReferenciada { get; set; }

        [Required]
        public bool NFeRefComplementarIsDevolucao { get; set; }

        [Required]
        public TipoOrdemVenda TipoOrdemVenda { get; set; }

        [Required]
        public TipoCompraVenda TipoVenda { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        [Required]
        public Guid ClienteId { get; set; }

        public Guid? GrupoTributarioPadraoId { get; set; }

        public Guid? TransportadoraId { get; set; }

        public TipoFrete TipoFrete { get; set; }

        [StringLength(60)]
        public string TipoEspecie { get; set; }

        [StringLength(7)]
        public string PlacaVeiculo { get; set; }

        public Guid? EstadoPlacaVeiculoId { get; set; }

        public string EstadoCodigoIbge { get; set; }

        public double? ValorFrete { get; set; }

        public double? PesoBruto { get; set; }

        public double? PesoLiquido { get; set; }

        public int? QuantidadeVolumes { get; set; }

        public Guid? FormaPagamentoId { get; set; }

        public Guid? CondicaoParcelamentoId { get; set; }

        public Guid? CategoriaId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataVencimento { get; set; }

        [Required]
        public bool MovimentaEstoque { get; set; }

        [Required]
        public bool AjusteEstoqueAutomatico { get; set; }

        [Required]
        public bool GeraFinanceiro { get; set; }

        [Required]
        public bool GeraNotaFiscal { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public double Total { get; set; }

        public double? TotalRetencoesServicos { get; set; }

        public double? TotalImpostosProdutos { get; set; }

        public double TotalImpostosProdutosNaoAgrega { get; set; }

        public double TotalImpostosServicosNaoAgrega { get; set; }

        [MaxLength(5000)]
        public string MensagemPadraoNota { get; set; }

        [MaxLength(1000)]
        public string InformacoesCompletamentaresNFS { get; set; }

        [StringLength(60)]
        public string NaturezaOperacao { get; set; }

        public TipoNfeComplementar TipoNfeComplementar { get; set; }

        public Guid? ContaFinanceiraParcelaPaiIdProdutos { get; set; }

        public Guid? ContaFinanceiraParcelaPaiIdServicos { get; set; }

        public Guid? CentroCustoId { get; set; }

        public Guid? UFSaidaPaisId { get; set; }

        [StringLength(60)]
        public string LocalEmbarque { get; set; }

        [StringLength(60)]
        public string LocalDespacho { get; set; }

        public bool RollbackMovimentaEstoque { get; set; }

        public bool RollbackGeraFinanceiro { get; set; }

        public virtual Pessoa Cliente { get; set; }

        public virtual GrupoTributario GrupoTributarioPadrao { get; set; }

        public virtual Pessoa Transportadora { get; set; }

        public virtual Estado EstadoPlacaVeiculo { get; set; }

        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }

        public virtual FormaPagamento FormaPagamento { get; set; }

        public virtual Categoria Categoria { get; set; }

        public virtual CentroCusto CentroCusto { get; set; }

        public virtual Estado UFSaidaPais { get; set; }


    }
}