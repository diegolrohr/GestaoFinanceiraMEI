using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Financeiro.Domain.Entities;

using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("conciliacaobancaria")]
    public class ConciliacaoBancariaController : ApiPlataformaController<ConciliacaoBancaria, ConciliacaoBancariaBL>
    {
    }
}