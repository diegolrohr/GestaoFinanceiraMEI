﻿using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("serienotafiscal")]
    public class SerieNotaFiscalController : ApiPlataformaController<SerieNotaFiscal, SerieNotaFiscalBL>
    {
        public SerieNotaFiscalController()
        {

        }
    }
}