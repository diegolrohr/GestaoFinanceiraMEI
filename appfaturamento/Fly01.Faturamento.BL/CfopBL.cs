using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.BL;

namespace Fly01.Faturamento.BL
{
    public class CfopBL : DomainBaseBL<Cfop>
    {
        public CfopBL(AppDataContext context) : base(context) { }
    }
}