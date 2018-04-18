using Fly01.Financeiro.Domain.Entities;
using Fly01.Core.BL;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class BancoBL : DomainBaseBL<Banco>
    {
        public BancoBL(DbContext context) : base(context)
        { }
    }
}
