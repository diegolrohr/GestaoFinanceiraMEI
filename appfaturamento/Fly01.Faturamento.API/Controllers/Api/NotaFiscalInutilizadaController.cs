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
    [ODataRoutePrefix("notafiscalinutilizada")]
    public class NotaFiscalInutilizadaController : ApiPlataformaController<NotaFiscalInutilizada, NotaFiscalInutilizadaBL>
    {
        public NotaFiscalInutilizadaController()
        {
        }

        public override async Task<IHttpActionResult> Post(NotaFiscalInutilizada entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var previous = unitOfWork.SerieNotaFiscalBL.All.Where(x => x.Id != entity.Id &&
                    x.Serie.ToUpper() == entity.Serie.ToUpper() &&
                    x.NumNotaFiscal == entity.NumNotaFiscal).ToList();

                    if (previous != null && previous.Any())
                    {
                        //previous.NumNotaFiscal++;

                        //unitOfWork.NotaFiscalBL.SerieNotaFiscalInutilizar(previous.Id);
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

                        await UnitSave();

                        if (MustProduceMessageServiceBus)
                            Producer<NotaFiscalInutilizada>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.EnHttpVerb.POST);

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