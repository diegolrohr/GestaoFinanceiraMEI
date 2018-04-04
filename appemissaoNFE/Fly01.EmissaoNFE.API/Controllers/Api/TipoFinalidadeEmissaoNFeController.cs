﻿using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.Controllers.API;
using Fly01.Core.Helpers;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("tipofinalidadeemissaonfe")]
    public class TipoFinalidadeEmissaoNFeController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoFinalidadeEmissaoNFe)));
        }

    }
}