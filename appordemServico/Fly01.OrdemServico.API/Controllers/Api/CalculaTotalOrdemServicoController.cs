using Fly01.Core.API;
using Fly01.Core.Notifications;
using Fly01.OrdemServico.BL;
using System;
using System.Web.Http;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [RoutePrefix("calculatotalordemServico")]
    public class CalculaTotalOrdemServicoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(Guid ordemServicoId, bool onList = false)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    return Ok(unitOfWork.OrdemServicoBL.CalculaTotalOrdemServico(ordemServicoId, onList));
                }
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }
    }
}