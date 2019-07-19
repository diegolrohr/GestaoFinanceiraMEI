using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Fly01.Compras.BL;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core;
using Fly01.Core.Rest;
using Newtonsoft.Json;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("atualizatotaisordemvenda")]
    public class AtualizaTotaisOrdemCompraController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult GetPlataforms()
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var plataformasOrdemCompra = (from ov in unitOfWork.OrdemCompraBL.AllWithoutPlatform.Where(x => x.Status == StatusOrdemCompra.Aberto)
                                                 group ov by new { ov.PlataformaId } into g
                                                 select new
                                                 {
                                                     plataformaId = g.Key.PlataformaId
                                                 });
                    foreach (var item in plataformasOrdemCompra)
                    {
                        try
                        {
                            var header = new Dictionary<string, string>()
                            {
                                { "AppUser", "fly01@totvs.com.br" },
                                { "PlataformaUrl", item.plataformaId }
                            };
                            var response = RestHelper.ExecutePostRequest<Object>("http://apicompras.bemacashlocal.com.br/api/", "atualizatotaisordemcompra/atualizatotaisasync", null, null, header, 300);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    };
                    return Ok(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        public async Task<IHttpActionResult> AtualizaTotaisAsync()
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {

                    var pedidos = unitOfWork.PedidoBL.All.Where(x => x.Status == StatusOrdemCompra.Aberto && x.Ativo == true);

                    foreach (Pedido item in pedidos)
                    {
                        try
                        {
                            item.Total = unitOfWork.PedidoBL.CalculaTotalOrdemCompra(item.Id, item.FornecedorId, item.GeraNotaFiscal, item.TipoCompra.ToString(), item.TipoFrete.ToString(), item.ValorFrete == null ? 0 : item.ValorFrete, false).Total;
                            await unitOfWork.Save();
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}