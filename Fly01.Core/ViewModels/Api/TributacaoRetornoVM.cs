namespace Fly01.Core.ViewModels.Api

{
    public class TributacaoRetornoVM
    {
        public TributacaoRetornoBaseVM Ipi { get; set; }

        public TributacaoRetornoBaseVM SubstituicaoTributaria { get; set; }

        public TributacaoRetornoBaseVM Icms { get; set; }

        public TributacaoRetornoBaseVM Fcp { get; set; }

        public TributacaoRetornoBaseVM FcpSt { get; set; }

        public DifalRetornoVM Difal { get; set; }
    }
}
