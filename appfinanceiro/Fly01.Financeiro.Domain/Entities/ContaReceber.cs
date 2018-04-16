using Fly01.Core.Entities.Domains.Enum;

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