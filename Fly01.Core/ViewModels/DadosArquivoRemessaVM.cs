using Boleto2Net;
using Fly01.Core.Entities.Domains.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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