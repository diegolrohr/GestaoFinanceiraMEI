using Fly01.Faturamento.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;

namespace Fly01.Faturamento.BL
{
    public class NBSBL : DomainBaseBL<Nbs>
    {
        public NBSBL(AppDataContext context) : base(context) { }
    }
}