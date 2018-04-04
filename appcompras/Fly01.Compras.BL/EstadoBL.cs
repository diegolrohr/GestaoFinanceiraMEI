using Fly01.Compras.DAL;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.Api.BL;

namespace Fly01.Compras.BL
{
    public class EstadoBL : DomainBaseBL<Estado>
    {
        public EstadoBL(AppDataContext context) : base(context)
        { }
    }
}
