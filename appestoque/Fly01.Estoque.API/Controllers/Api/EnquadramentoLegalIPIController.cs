using Fly01.Estoque.BL;
using Fly01.Estoque.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Routing;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("enquadramentolegalipi")]
    public class EnquadramentoLegalIPIController : ApiDomainController<EnquadramentoLegalIPI, EnquadramentoLegalIPIBL>
    {
    }
}