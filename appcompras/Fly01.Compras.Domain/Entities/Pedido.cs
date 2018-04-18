using System;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.Domain.Entities
{
    public class Pedido : OrdemCompra
    {
        public Pedido()
        {
            TipoOrdemCompra = TipoOrdemCompra.Pedido;
        }

        [Required]
        public Guid FornecedorId { get; set; }

        public Guid? TransportadoraId { get; set; }

        public TipoFrete TipoFrete { get; set; }

        public double? ValorFrete { get; set; }

        public double? PesoBruto { get; set; }

        public double? PesoLiquido { get; set; }

        public int? QuantidadeVolumes { get; set; }

        [Required]
        public bool MovimentaEstoque { get; set; }

        [Required]
        public bool GeraFinanceiro { get; set; }

        public Guid? OrcamentoOrigemId { get; set; }

        #region Navigations Properties

        public virtual Pessoa Fornecedor { get; set; }
        public virtual Pessoa Transportadora { get; set; }
        public virtual Orcamento OrcamentoOrigem { get; set; }

        #endregion
    }
}
