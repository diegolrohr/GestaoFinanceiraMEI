using System.Web.Http;
using Fly01.Faturamento.BL;
using System.Linq;
using Fly01.Core.API;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("kitordemvenda")]
    public class KitOrdemVendaController : ProdutoServicoBaseController
    {
        public IHttpActionResult Post(UtilizarKitVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.OrdemVendaBL.UtilizarKitOrdemVenda(entity);

                return Ok();
            }
        }
    }
}