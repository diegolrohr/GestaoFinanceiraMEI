﻿using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.API;
using System.Web.Http;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("avaliacaoapp")]
    public class AvaliacaoAppController : ApiPlataformaMongoBaseController<AvaliacaoApp>
    {
    }
}