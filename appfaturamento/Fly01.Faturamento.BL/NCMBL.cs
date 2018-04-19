using Fly01.Faturamento.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.BL
{
    public class NCMBL : DomainBaseBL<Ncm>
    {
        public NCMBL(AppDataContext context) : base(context) { }
    }
}