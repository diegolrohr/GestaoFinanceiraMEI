using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFe : NotaFiscal
    {
        public NFe()
        {
            TipoNotaFiscal = TipoNotaFiscal.NFe;
        }
        
        public double TotalImpostosProdutos { get; set; }

        public double TotalImpostosProdutosNaoAgrega { get; set; }

        public TipoNfeComplementar TipoNfeComplementar { get; set; }
    }
}