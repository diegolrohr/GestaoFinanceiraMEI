using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Compras.Domain.Entities
{
    public class ContaPagar : ContaFinanceira
    {
        public override TipoContaFinanceira GetTipoContaFinceira()
        {
            return TipoContaFinanceira.ContaPagar;
        }
    }
}
