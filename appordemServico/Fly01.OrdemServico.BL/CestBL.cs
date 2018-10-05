using Fly01.OrdemServico.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.OrdemServico.BL
{
    public class CestBL : DomainBaseBL<Cest>
    {
        public CestBL(AppDataContext context) : base(context) { }
    }
}