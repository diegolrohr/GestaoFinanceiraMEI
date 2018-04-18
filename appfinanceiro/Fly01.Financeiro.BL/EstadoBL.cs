using Fly01.Core.BL;using Fly01.Core.Entities.Domains.Commons;

using Fly01.Financeiro.API.Models.DAL;
using Fly01.Financeiro.Domain.Entities;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class EstadoBL : DomainBaseBL<Estado>
    {
        public EstadoBL(AppDataContext context) : base(context)
        { }
    }
}
