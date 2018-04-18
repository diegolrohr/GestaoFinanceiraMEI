using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.BL;

namespace Fly01.Faturamento.BL
{
    public class NBSBL : DomainBaseBL<NBS>
    {
        public NBSBL(AppDataContext context) : base(context) { }
    }
}