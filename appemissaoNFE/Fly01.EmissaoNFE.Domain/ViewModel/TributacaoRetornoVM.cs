﻿using System;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class TributacaoRetornoVM
    {
        public TributacaoRetornoBaseVM Ipi { get; set; }

        public TributacaoRetornoBaseVM SubstituicaoTributaria { get; set; }

        public TributacaoRetornoBaseVM Icms { get; set; }

        public TributacaoRetornoBaseVM Fcp { get; set; }

        public DifalRetornoVM Difal { get; set; }
    }
}
