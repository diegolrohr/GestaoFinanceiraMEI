using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Compras.Domain.Entities
{
    public class Orcamento : OrdemCompra
    {
        public Orcamento()
        {
            TipoOrdemCompra = TipoOrdemCompra.Orcamento;
        }
    }
}