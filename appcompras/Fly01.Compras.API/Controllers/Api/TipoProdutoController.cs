﻿using System.Web.Http;
using Fly01.Compras.Domain.Enums;
using Fly01.Core.API;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("tipoproduto")]
    public class TipoProdutoController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoProduto)));
        }
    }
}