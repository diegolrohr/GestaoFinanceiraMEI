﻿using Fly01.Estoque.BL;
using System.Web.OData.Routing;
using Fly01.Estoque.Domain.Entities;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("tipomovimento")]
    public class TipoMovimentoController : ApiPlataformaController<TipoMovimento, TipoMovimentoBL>
    {
        public TipoMovimentoController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}