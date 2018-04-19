﻿using Fly01.Compras.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("ordemcompra")]
    public class OrdemCompraController : ApiPlataformaController<OrdemCompra, OrdemCompraBL>
    {
    }
}