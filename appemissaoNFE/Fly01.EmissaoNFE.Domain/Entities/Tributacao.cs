using Fly01.Core.Domain;

namespace Fly01.EmissaoNFE.Domain
{
    public class Tributacao : PlataformaBase
    {
        public double ValorBase { get; set; }
        public double ValorDespesa { get; set; }
        public double ValorFrete { get; set; }

        public bool ConsumidorFinal { get; set; }
        public bool InscricaoEstadual { get; set; }

        public Ipi Ipi { get; set; }

        public SubstituicaoTributaria SubstituicaoTributaria { get; set; }
        
        public Icms Icms { get; set; }

        public Fcp Fcp { get; set; }
    }
}
