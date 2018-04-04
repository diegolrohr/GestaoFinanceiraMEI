using Fly01.Compras.Domain.Enums;

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
