using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ContaReceber : ContaFinanceira
    {
        public override TipoContaFinanceira GetTipoContaFinceira() => TipoContaFinanceira.ContaReceber;
    }
}