using Fly01.Core.BL;
using System.Data.Entity;
using Fly01.Estoque.Domain.Entities;

namespace Fly01.Estoque.BL
{
    public class UnidadeMedidaBL : DomainBaseBL<UnidadeMedida>
    {
        public UnidadeMedidaBL(DbContext context) : base(context) { }
    }
}
