using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.Api.BL;

namespace Fly01.Faturamento.BL
{
    public class NCMBL : DomainBaseBL<NCM>
    {
        public NCMBL(AppDataContext context) : base(context) { }
    }
}