﻿using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Financeiro.Domain.Entities;
using System.Web.Http;
using System.Linq;
using System.Web.OData;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("categoria")]
    public class CategoriaController : ApiPlataformaController<Categoria, CategoriaBL>
    {
        public CategoriaController()
        {
            MustProduceMessageServiceBus = true;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public override IHttpActionResult Get()
        {
            var a = All().ToList();

            return Ok(a);
        }
    }
}