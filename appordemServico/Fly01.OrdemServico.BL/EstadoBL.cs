using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.OrdemServico.API.Models.DAL;

namespace Fly01.OrdemServico.BL
{
    public class EstadoBL : DomainBaseBL<Estado>
    {
        public EstadoBL(AppDataContext context) : base(context)
        { }
    }
}
