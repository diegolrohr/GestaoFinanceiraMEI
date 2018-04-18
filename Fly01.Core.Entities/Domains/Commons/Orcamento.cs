using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Orcamento : OrdemCompra
    {
        public Orcamento()
        {
            TipoOrdemCompra = TipoOrdemCompra.Orcamento;
        }
    }
}