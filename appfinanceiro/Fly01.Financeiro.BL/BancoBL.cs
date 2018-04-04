using Fly01.Financeiro.Domain.Entities;
using Fly01.Core.Api.BL;
using System.Data.Entity;

namespace Fly01.Financeiro.BL
{
    public class BancoBL : DomainBaseBL<Banco>
    {
        public BancoBL(DbContext context) : base(context)
        { }
    }
}
