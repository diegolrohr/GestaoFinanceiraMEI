using System;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class TributacaoRetornoBaseVM
    {
        public double Base { get; set; }
        public double Aliquota { get; set; }
        public double Valor { get; set; }
        public bool AgregaTotalNota { get; set; }
    }
}
