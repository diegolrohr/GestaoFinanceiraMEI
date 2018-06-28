using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Web.Http;
using System.Threading.Tasks;
using Fly01.Core.ServiceBus;
using System.Data.Entity;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;
using System;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("serienotafiscalinutilizada")]
    public class SerieNotaFiscalInutilizadaController : ApiPlataformaController<SerieNotaFiscal, SerieNotaFiscalInutilizadaBL>
    {
        public SerieNotaFiscalInutilizadaController()
        {
        }

        public override async Task<IHttpActionResult> Post(SerieNotaFiscal entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var previous = unitOfWork.SerieNotaFiscalBL.All.Where(x => x.Id != entity.Id &&
                    x.Serie.ToUpper() == entity.Serie.ToUpper() &&
                    x.NumNotaFiscal == entity.NumNotaFiscal &&
                    (x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || entity.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || x.TipoOperacaoSerieNotaFiscal == entity.TipoOperacaoSerieNotaFiscal) &&
                    x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Habilitada).FirstOrDefault();

                    if (previous != null)
                    {
                        previous.NumNotaFiscal++;
                        unitOfWork.NotaFiscalBL.SerieNotaFiscalInutilizar(previous.Id);
                        await UnitSave();
                        return Ok();
                    }
                    else
                    {
                        ModelState.Clear();

                        Insert(entity);

                        Validate(entity);

                        if (!ModelState.IsValid)
                            AddErrorModelState(ModelState);

                        unitOfWork.NotaFiscalBL.SerieNotaFiscalInutilizar(entity.Id);

                        await UnitSave();

                        if (MustProduceMessageServiceBus)
                            Producer<SerieNotaFiscal>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.EnHttpVerb.POST);

                        return Created(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }            
        }
    }
}