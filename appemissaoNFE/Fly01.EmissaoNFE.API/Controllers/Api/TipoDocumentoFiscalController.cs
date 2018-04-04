﻿using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.Controllers.API;
using Fly01.Core.Helpers;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("tipodocumentofiscal")]
    public class TipoDocumentoFiscalController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok(EnumHelper.GetDataEnumValues(typeof(TipoNota)));
        }

    }
}