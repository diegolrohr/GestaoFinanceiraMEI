using Fly01.Compras.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Web.Http;
using System.Threading.Tasks;
using Fly01.Core.ServiceBus;
using System;
using Fly01.Core.Notifications;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("notafiscalinutilizada")]
    public class NotaFiscalInutilizadaController : ApiPlataformaController<NotaFiscalInutilizada, NotaFiscalInutilizadaBL>
    {
        public NotaFiscalInutilizadaController()
        {
            MustProduceMessageServiceBus = true;
        }

        public override async Task<IHttpActionResult> Post(NotaFiscalInutilizada entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            try
            {
                ModelState.Clear();

                UnitOfWork.NotaFiscalInutilizadaBL.ValidaModel(entity);

                if (entity.IsValid())
                {
                    UnitOfWork.NotaFiscalEntradaBL.NotaFiscalInutilizar(entity);
                }

                Insert(entity);

                Validate(entity);

                if (!ModelState.IsValid)
                    AddErrorModelState(ModelState);

                await UnitSave();

                if (MustProduceMessageServiceBus)
                    Producer<NotaFiscalInutilizada>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.EnHttpVerb.POST);

                return Created(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}