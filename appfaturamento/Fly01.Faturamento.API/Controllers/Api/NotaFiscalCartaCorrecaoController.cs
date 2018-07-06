using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Faturamento.BL;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Routing;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("notafiscalcartacorrecao")]
    public class NotaFiscalCartaCorrecaoController : ApiPlataformaController<NotaFiscalCartaCorrecao, NotaFiscalCartaCorrecaoBL>
    {

        public NotaFiscalCartaCorrecaoController()
        {
        }

        public override async Task<IHttpActionResult> Post(NotaFiscalCartaCorrecao entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            try
            {
                ModelState.Clear();

                UnitOfWork.NotaFiscalCartaCorrecaoBL.ValidaModel(entity);

                //if (entity.IsValid())
                //{
                //    UnitOfWork.NotaFiscalBL.NotaFiscalInutilizar(entity);
                //}

                Insert(entity);

                Validate(entity);

                if (!ModelState.IsValid)
                    AddErrorModelState(ModelState);

                await UnitSave();

                return Created(entity);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
