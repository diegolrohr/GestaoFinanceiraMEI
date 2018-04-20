﻿using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("tiposubstituicaotributaria")]
    public class TipoSubstituicaoTributariaController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoSubstituicaoTributaria)));
        }
        
    }
}