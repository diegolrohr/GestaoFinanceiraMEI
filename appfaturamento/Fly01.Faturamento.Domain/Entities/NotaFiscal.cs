using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.Api.Domain;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Faturamento.Domain.Entities
{
    public class NotaFiscal : PlataformaBase
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Numero { get; set; }

        [Required]
        public Guid OrdemVendaOrigemId { get; set; }

        [Required]
        public TipoNotaFiscal TipoNotaFiscal { get; set; }

        [Required]
        public TipoVenda TipoVenda { get; set; }

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

        [StringLength(7)]
        public string PlacaVeiculo { get; set; }

        public Guid? EstadoPlacaVeiculoId { get; set; }

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

        [StringLength(50)]
        public string SefazId { get; set; }

        #region Navigation Properties

        public virtual OrdemVenda OrdemVendaOrigem { get; set; }
        public virtual Pessoa Cliente { get; set; }
        public virtual Pessoa Transportadora { get; set; }
        public virtual Estado EstadoPlacaVeiculo { get; set; }
        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual SerieNotaFiscal SerieNotaFiscal { get; set; }

        #endregion

    }
}
