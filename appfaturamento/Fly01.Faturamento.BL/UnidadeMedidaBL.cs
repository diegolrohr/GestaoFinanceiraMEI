using System.Data.Entity;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;


namespace Fly01.Faturamento.BL
{
    public class UnidadeMedidaBL : DomainBaseBL<UnidadeMedida>
    {
        public UnidadeMedidaBL(DbContext context) : base(context) { }
    }
}