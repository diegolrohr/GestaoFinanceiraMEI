﻿using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("kit")]
    public class KitController : ApiPlataformaController<Kit, KitBL>
    {
        public KitController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}