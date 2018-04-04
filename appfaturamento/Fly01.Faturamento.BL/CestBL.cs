using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.Api.BL;

namespace Fly01.Faturamento.BL
{
    public class CestBL : DomainBaseBL<Cest>
    {
        public CestBL(AppDataContext context) : base(context) { }
    }
}