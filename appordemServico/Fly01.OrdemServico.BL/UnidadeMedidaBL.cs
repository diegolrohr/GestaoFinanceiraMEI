using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using System.Data.Entity;

namespace Fly01.OrdemServico.BL
{
    public class UnidadeMedidaBL : DomainBaseBL<UnidadeMedida>
    {
        public UnidadeMedidaBL(DbContext context) : base(context) { }
    }
}
