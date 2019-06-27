using System.Web.Http;
using Fly01.Compras.BL;
using System;
using System.Threading.Tasks;
using Fly01.Core.API;
using Fly01.Core.Notifications;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("notafiscalxml")]
    public class NotaFiscalXMLController : ApiBaseController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var xml = unitOfWork.NotaFiscalEntradaBL.NotaFiscalXML(id);
                    await unitOfWork.Save();
                    return Ok(xml);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var xml = unitOfWork.NotaFiscalEntradaBL.NotasFiscaisPeriodo(dataInicial, dataFinal);
                    await unitOfWork.Save();
                    return Ok(xml);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}