using Fly01.Faturamento.Domain.Enums;

namespace Fly01.Faturamento.Domain.Entities
{
    public class NFe : NotaFiscal
    {
        public NFe()
        {
            TipoNotaFiscal = TipoNotaFiscal.NFe;
        }
        
        public double TotalImpostosProdutos { get; set; }

        public double TotalImpostosProdutosNaoAgrega { get; set; }
    }
}
