using Fly01.Compras.BL;
using Fly01.Core.API;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fly01.Compras.API.Controllers.Api
{
    public class NFeImportacaoVinculacaoController : ApiBaseController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    unitOfWork.NFeImportacaoBL.AtualizarVinculacoes(id);
                    await unitOfWork.Save();

                    var item = unitOfWork.NFeImportacaoBL.AllIncluding(
                                x => x.Fornecedor,
                                x => x.Transportadora,
                                x => x.FormaPagamento,
                                x => x.CondicaoParcelamento,
                                x => x.Categoria,
                                x => x.Pedido).Where(x => x.Id == id).FirstOrDefault();

                    
                    return Ok(item);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}