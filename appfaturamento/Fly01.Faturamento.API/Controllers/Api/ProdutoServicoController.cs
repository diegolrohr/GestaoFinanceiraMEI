using System.Web.Http;
using Fly01.Faturamento.BL;
using System.Linq;
using Fly01.Core.API;
using System;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("produtoservico")]
    public class ProdutoServicoController : ProdutoServicoBaseController
    {
        public IHttpActionResult Get(Guid kitId, string filtro = "")
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var result = GetProdutosServicos(filtro, unitOfWork.ProdutoBL.All, unitOfWork.ServicoBL.All, unitOfWork.KitItemBL.All.Where(x => x.KitId == kitId));

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