using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.BL
{
    public class EstadoBL : DomainBaseBL<Estado>
    {
        public EstadoBL(AppDataContext context) : base(context)
        { }
    }
}
