using Fly01.OrdemServico.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;

namespace Fly01.OrdemServico.BL
{
    public class ISSBL : DomainBaseBL<Iss>
    {
        public ISSBL(AppDataContext context) : base(context) { }
    }
}