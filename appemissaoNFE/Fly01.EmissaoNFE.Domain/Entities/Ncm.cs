using Fly01.Core.Entities.Domains;

namespace Fly01.EmissaoNFE.Domain
{
    public class NCM : DomainBase
    {
        public string Codigo { get; set; }
        
        public string Descricao { get; set; }

        public double AliquotaIPI { get; set; }
    }
}
