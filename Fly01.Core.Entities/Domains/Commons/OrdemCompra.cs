using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class OrdemCompra : PlataformaBase
    {
        [Required]
        public int Numero { get; set; }

        [StringLength(44)]
        public string ChaveNFeReferenciada { get; set; }

        [Required]
        public TipoOrdemCompra TipoOrdemCompra { get; set; }

        [Required]
        public StatusOrdemCompra Status { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public Guid? GrupoTributarioPadraoId { get; set; }

        [StringLength(7)]
        public string PlacaVeiculo { get; set; }

        public Guid? EstadoPlacaVeiculoId { get; set; }

        public string EstadoCodigoIbge { get; set; }

        public Guid? FormaPagamentoId { get; set; }

        public Guid? CondicaoParcelamentoId { get; set; }

        public Guid? CategoriaId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataVencimento { get; set; }

        [Required]
        public bool AjusteEstoqueAutomatico { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public double? TotalImpostosProdutos { get; set; }

        public double TotalImpostosProdutosNaoAgrega { get; set; }

        public double Total { get; set; }

        [MaxLength(5000)]
        public string MensagemPadraoNota { get; set; }

        [StringLength(60)]
        public string NaturezaOperacao { get; set; }

        public virtual Estado EstadoPlacaVeiculo { get; set; }
        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual GrupoTributario GrupoTributarioPadrao { get; set; }
    }
}
