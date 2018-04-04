using Fly01.Financeiro.Domain.Enums;

namespace Fly01.Financeiro.Domain.Entities
{
    public class ContaReceber : ContaFinanceira
    {
        public override TipoContaFinanceira GetTipoContaFinceira()
        {
            return TipoContaFinanceira.ContaReceber;
        }
    }
}