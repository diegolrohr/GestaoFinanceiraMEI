﻿using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Financeiro.Domain.Entities;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("transferencia")]
    public class TransferenciaController : ApiPlataformaController<Transferencia, TransferenciaBL> { }
}