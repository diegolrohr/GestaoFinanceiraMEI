using Fly01.Core.Entities.Domains.Commons;
using Fly01.OrdemServico.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData.Routing;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [ODataRoutePrefix("unidademedida")]
    public class UnidadeMedidaController : ApiDomainController<UnidadeMedida, UnidadeMedidaBL>
    {
    }
}
