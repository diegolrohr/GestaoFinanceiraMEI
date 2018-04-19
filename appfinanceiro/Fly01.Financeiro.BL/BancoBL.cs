using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using System.Data.Entity;

namespace Fly01.Financeiro.BL
{
    public class BancoBL : DomainBaseBL<Banco>
    {
        public BancoBL(DbContext context) : base(context)
        { }
    }
}
