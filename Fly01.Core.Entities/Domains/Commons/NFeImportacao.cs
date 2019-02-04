using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFeImportacao : PlataformaBase
    {
        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Xml { get; set; }

        [Required]
        [MaxLength(32)]
        public string XmlMd5 { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataEmissao { get; set; }

        [Required]
        [MaxLength(3)]
        public string Serie { get; set; }

        public int Numero { get; set; }

        public TipoCompraVenda Tipo { get; set; }

        public Status Status { get; set; }

        public bool AtualizaDadosFornecedor { get; set; }

        public bool NovoFornecedor { get; set; }

        public Guid? FornecedorId { get; set; }

        public TipoFrete TipoFrete { get; set; }

        public bool AtualizaDadosTransportadora { get; set; }

        public bool NovaTransportadora { get; set; }

        public Guid? TransportadoraId { get; set; }

        public bool GeraFinanceiro { get; set; }

        public bool GeraContasXml { get; set; }

        public Guid? CondicaoParcelamentoId { get; set; }

        public Guid? FormaPagamentoId { get; set; }

        public Guid? CategoriaId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataVencimento { get; set; }

        public double ValorTotal { get; set; }

        public double ValorFrete { get; set; }

        public double SomatorioICMSST { get; set; }

        public double SomatorioIPI { get; set; }

        public double SomatorioFCPST { get; set; }

        public double SomatorioDesconto { get; set; }

        public double SomatorioProduto { get; set; }

        public Guid? ContaFinanceiraPaiId { get; set; }

        public bool NovoPedido { get; set; }

        /// <summary>
        /// One-to-Zero-or-One Relationship
        /// </summary>
        [ForeignKey("Pedido")]
        public Guid? PedidoId { get; set; }

        #region FKS
        public virtual Pedido Pedido { get; set; }
        public virtual Pessoa Fornecedor { get; set; }
        public virtual Pessoa Transportadora { get; set; }
        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual Categoria Categoria { get; set; }
        #endregion
    }
}