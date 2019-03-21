using Fly01.Core.Entities.Domains;

namespace Fly01.EmissaoNFE.Domain.Entities
{
    public class ResponsavelTecnico : DomainBase
    {
        public string CNPJ { get; set; }
        public string Contato { get; set; }
        public string Email { get; set; }
        public string Fone { get; set; }
        public string IdentificadorCodigoResponsavelTecnico { get; set; }
        public string CodigoResponsavelTecnico { get; set; }
        public string HashCSRT { get; set; }
    }
}
