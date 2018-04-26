using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("cnab")]
    public class CnabController : ApiPlataformaController<Cnab, CnabBL>
    {
    }
}