using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Pedido : OrdemCompra
    {
        public Pedido()
        {
            TipoOrdemCompra = TipoOrdemCompra.Pedido;
        }

        public double? ValorFrete { get; set; }

        public double? PesoBruto { get; set; }

        public double? PesoLiquido { get; set; }

        [Required]
        public bool GeraFinanceiro { get; set; }

        [Required]
        public bool MovimentaEstoque { get; set; }

        public int? QuantidadeVolumes { get; set; }

        public TipoFrete TipoFrete { get; set; }

        public Guid FornecedorId { get; set; }

        public Guid? TransportadoraId { get; set; }

        public Guid? OrcamentoOrigemId { get; set; }

        public virtual Pessoa Transportadora { get; set; }
        public virtual Pessoa Fornecedor { get; set; }
        public virtual Orcamento OrcamentoOrigem { get; set; }
    }
}
