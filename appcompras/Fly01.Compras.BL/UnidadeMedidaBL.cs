using System.Data.Entity;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.Api.BL;

namespace Fly01.Compras.BL
{
    public class UnidadeMedidaBL : DomainBaseBL<UnidadeMedida>
    {
        public UnidadeMedidaBL(DbContext context) : base(context) { }
    }
}