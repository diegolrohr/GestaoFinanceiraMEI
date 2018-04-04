using System.Web.OData.Routing;
using Fly01.Faturamento.BL;
using Fly01.Faturamento.Domain.Entities;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("cfop")]
    public class CfopController : ApiDomainController<Cfop, CfopBL>
    {
    }
}