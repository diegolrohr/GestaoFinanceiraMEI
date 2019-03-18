namespace Fly01.Core.Entities.Domains.Commons
{
    public class ResponsavelTecnico : PlataformaBase
    {
        public string CNPJ { get; set; }
        public string Contato { get; set; }
        public string email { get; set; }
        public string fone { get; set; }
        public string IdentificadorCodigoResponsavelTecnico { get; set; }
        public string CodigoResponsavelTecnico { get; set; }
        public string ChaveAcessoNFE { get; set; }
    }
}
