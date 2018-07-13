using System;

namespace Fly01.Core.ViewModels.Presentation
{
    public class DadosArquivoRemessaVM
    {
        public Guid ContaBancariaCedenteId { get; set; }
        public int CodigoBanco { get; set; }
        public int TotalBoletosGerados { get; set; }
        public double ValorTotalArquivoRemessa { get; set; }
        public byte[] ConteudoArquivoRemessa { get; set; }
    }
}