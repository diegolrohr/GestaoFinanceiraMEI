using System.Web.Http;
using Fly01.OrdemServico.BL;
using System.Linq;
using Fly01.Core.API;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [RoutePrefix("produtoservico")]
    public class ProdutoServicoController : ProdutoServicoBaseController
    {
        public IHttpActionResult Get(string filtro = "")
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var result = GetProdutosServicos(filtro, unitOfWork.ProdutoBL.All, unitOfWork.ServicoBL.All);

                return Ok(
                    new
                    {
                        value = result.ToList()
                    }
                );
            }
        }
    }
}