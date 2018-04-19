﻿using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("movimentacao")]
    public class MovimentacaoController : ApiPlataformaController<MovimentacaoFinanceira, MovimentacaoBL>
    {
    }
}