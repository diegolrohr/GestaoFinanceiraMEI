using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Faturamento.Domain.Entities
{
    public class NFSe : NotaFiscal
    {
        public NFSe()
        {
            TipoNotaFiscal = TipoNotaFiscal.NFSe;
        }

        public double TotalImpostosServicos { get; set; }
    }
}
