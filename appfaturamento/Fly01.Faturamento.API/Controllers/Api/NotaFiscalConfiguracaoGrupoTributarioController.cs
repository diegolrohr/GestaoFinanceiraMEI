using System.Web.Http;
using Fly01.Faturamento.BL;
using System;
using Fly01.Core.API;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("notafiscalconfiguracaogrupotributario")]
    public class NotaFiscalConfiguracaoGrupoTributarioController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid notaFiscalId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.NotaFiscalItemTributacaoBL.GetConfiguracoesGrupoTributarioFinalizacaoPedido(notaFiscalId));
            }
        }
    }
}