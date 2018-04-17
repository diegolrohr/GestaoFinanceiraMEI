using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.Domain.Entities
{
    public class Cest : CestBase
    {
        public virtual NCM Ncm { get; set; }
    }
}