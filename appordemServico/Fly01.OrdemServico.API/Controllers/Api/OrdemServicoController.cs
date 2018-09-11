using Fly01.OrdemServico.BL;
using System.Web.OData.Routing;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [ODataRoutePrefix("ordemservico")]
    public class OrdemServicoController : ApiPlataformaController<Core.Entities.Domains.Commons.OrdemServico, OrdemServicoBL>
    {
        public OrdemServicoController()
        {
            MustExecuteAfterSave = true;
        }
    }
}