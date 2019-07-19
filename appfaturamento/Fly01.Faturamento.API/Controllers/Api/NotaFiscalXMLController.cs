using System.Web.Http;
using Fly01.Faturamento.BL;
using System;
using System.Threading.Tasks;
using Fly01.Core.API;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
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
                    var xml = unitOfWork.NotaFiscalBL.NotaFiscalXML(id);
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
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    string idsXML = "";
                    foreach (NotaFiscal item in unitOfWork.NotaFiscalBL.NotasFiscaisPeriodo(dataInicial, dataFinal))
                    {
                        if (item.Id != null)
                        {
                            idsXML += item.Id + ",";
                        }
                    }
                    return Ok(new { idsXML = idsXML});
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}