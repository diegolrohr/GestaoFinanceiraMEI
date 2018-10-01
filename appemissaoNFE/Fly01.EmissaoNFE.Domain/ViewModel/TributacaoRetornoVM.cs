﻿namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class TributacaoRetornoVM
    {
        public TributacaoRetornoBaseVM Ipi { get; set; }

        public TributacaoRetornoBaseVM SubstituicaoTributaria { get; set; }

        public TributacaoRetornoBaseVM Icms { get; set; }

        public TributacaoRetornoBaseVM Fcp { get; set; }

        public TributacaoRetornoBaseVM FcpSt { get; set; }

        public DifalRetornoVM Difal { get; set; }

        public TributacaoRetornoBaseVM Pis { get; set; }

        public TributacaoRetornoBaseVM Cofins { get; set; }

        public TributacaoRetornoBaseVM Iss { get; set; }

        public TributacaoRetornoBaseVM Inss { get; set; }

        public TributacaoRetornoBaseVM ImpostoRenda { get; set; }

        public TributacaoRetornoBaseVM Csll { get; set; }
    }
}
