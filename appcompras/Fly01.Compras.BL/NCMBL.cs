using Fly01.Compras.DAL;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.Api.BL;

namespace Fly01.Compras.BL
{
    public class NCMBL : DomainBaseBL<NCM>
    {
        public NCMBL(AppDataContext context) : base(context) { }
    }
}