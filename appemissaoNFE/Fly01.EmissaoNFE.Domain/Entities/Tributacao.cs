using Fly01.Core.Entities.Domains;

namespace Fly01.EmissaoNFE.Domain
{
    public class Tributacao : PlataformaBase
    {
        public double ValorBase { get; set; }
        public double ValorDespesa { get; set; }
        public double ValorFrete { get; set; }

        public bool ConsumidorFinal { get; set; }
        public bool InscricaoEstadual { get; set; }
        public bool SimplesNacional { get; set; }

        public Ipi Ipi { get; set; }

        public SubstituicaoTributaria SubstituicaoTributaria { get; set; }
        
        public Icms Icms { get; set; }

        public Fcp Fcp { get; set; }

        public FcpSt FcpSt { get; set; }

        public Pis Pis { get; set; }

        public Cofins Cofins { get; set; }

        public Inss Inss { get; set; }

        public ImpostoRenda ImpostoRenda { get; set; }
    }
}
