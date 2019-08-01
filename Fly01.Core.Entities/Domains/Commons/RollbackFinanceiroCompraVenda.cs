using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class RollbackFinanceiroCompraVenda : PlataformaBase
    {
        public Guid? ContaFinanceiraParcelaPaiIdProdutos { get; set; }
        public Guid? ContaFinanceiraParcelaPaiIdServicos { get; set; }
        public Guid? TransportadoraId { get; set; }
        public int NumeroPedido { get; set; }

    }
}