using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("cnab")]
    public class CnabController : ApiPlataformaController<Cnab, CnabBL>
    {
        [HttpGet]
        [Route("imprimeBoleto")]
        //public void ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId, DateTime dataDesconto, double valorDesconto)
        public IHttpActionResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            var cnabBL = 1;
            var cnabBL1 = cnabBL;
            return null;
        }
    }
}