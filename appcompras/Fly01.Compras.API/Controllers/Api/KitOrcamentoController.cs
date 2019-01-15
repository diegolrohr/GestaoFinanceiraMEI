using Fly01.Compras.BL;
using Fly01.Core.API;
using Fly01.Core.Notifications;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("kitorcamento")]
    public class KitOrcamentoController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post(UtilizarKitVM entity)
        {
            if (entity.FornecedorPadraoId == null || entity.FornecedorPadraoId == default(Guid))
            {
                throw new BusinessException("Informe o fornecedor padrão para adicionar os produtos do kit ao orçamento.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.OrcamentoBL.UtilizarKitOrcamento(entity);
                await unitOfWork.Save();
            }
            return Ok(new { success = true });
        }
    }
}