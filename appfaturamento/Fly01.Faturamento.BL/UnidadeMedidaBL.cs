using System.Data.Entity;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.BL;

namespace Fly01.Faturamento.BL
{
    public class UnidadeMedidaBL : DomainBaseBL<UnidadeMedida>
    {
        public UnidadeMedidaBL(DbContext context) : base(context) { }
    }
}