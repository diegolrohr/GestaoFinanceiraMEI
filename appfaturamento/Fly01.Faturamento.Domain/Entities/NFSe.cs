using Fly01.Faturamento.Domain.Enums;

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
