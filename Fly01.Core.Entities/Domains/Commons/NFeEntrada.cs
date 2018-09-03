using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFeEntrada : NotaFiscalEntrada
    {
        public NFeEntrada()
        {
            TipoNotaFiscal = TipoNotaFiscal.NFe;
        }
        
        public double TotalImpostosProdutos { get; set; }

        public double TotalImpostosProdutosNaoAgrega { get; set; }
    }
}