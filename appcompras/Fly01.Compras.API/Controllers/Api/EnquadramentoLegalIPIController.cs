using Fly01.Compras.BL;
using Fly01.Compras.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Routing;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("enquadramentolegalipi")]
    public class EnquadramentoLegalIPIController : ApiDomainController<EnquadramentoLegalIPI, EnquadramentoLegalIPIBL>
    {
    }
}