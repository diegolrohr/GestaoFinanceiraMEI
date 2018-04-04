using Fly01.Faturamento.Domain.Enums;

namespace Fly01.Faturamento.Domain.Entities
{
    public class ContaReceber : ContaFinanceira
    {
        public override TipoContaFinanceira GetTipoContaFinceira()
        {
            return TipoContaFinanceira.ContaReceber;
        }
    }
}
