using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Pedido : OrdemCompra
    {
        public Pedido()
        {
            TipoOrdemCompra = TipoOrdemCompra.Pedido;
        }

        [StringLength(60)]
        public string NumeracaoVolumesTrans { get; set; }

        [StringLength(60)]
        public string Marca { get; set; }

        [StringLength(60)]
        public string TipoEspecie { get; set; }

        [Required]
        public TipoCompraVenda TipoCompra { get; set; }
        public double? ValorFrete { get; set; }

        public double? PesoBruto { get; set; }

        public double? PesoLiquido { get; set; }

        [Required]
        public bool GeraFinanceiro { get; set; }

        [Required]
        public bool GeraNotaFiscal { get; set; }

        [Required]
        public bool MovimentaEstoque { get; set; }

        public int? QuantidadeVolumes { get; set; }

        public TipoFrete TipoFrete { get; set; }

        public Guid FornecedorId { get; set; }

        public Guid? TransportadoraId { get; set; }

        public Guid? OrcamentoOrigemId { get; set; }

        public Guid? ContaFinanceiraParcelaPaiId { get; set; }

        public Guid? NFeImportacaoId { get; set; }

        public virtual Pessoa Transportadora { get; set; }
        public virtual Pessoa Fornecedor { get; set; }
        public virtual Orcamento OrcamentoOrigem { get; set; }
        //public virtual NFeImportacao NFeImportacao { get; set; }
    }
}
