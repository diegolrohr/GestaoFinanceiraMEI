using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Fly01.Faturamento.BL;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core;
using Fly01.Core.Rest;
using Newtonsoft.Json;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("atualizatotaisordemvenda")]
    public class AtualizaTotaisOrdemVendaController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult GetPlataforms()
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var plataformasOrdemVenda = (from ov in unitOfWork.OrdemVendaBL.AllWithoutPlatform.Where(x => x.GeraNotaFiscal == true && x.Status == Status.Aberto)
                                                 group ov by new { ov.PlataformaId } into g
                                                 select new
                                                 {
                                                     plataformaId = g.Key.PlataformaId
                                                 });
                    foreach (var item in plataformasOrdemVenda)
                    {
                        try
                        {
                            var header = new Dictionary<string, string>()
                            {
                                { "AppUser", "fly01@totvs.com.br" },
                                { "PlataformaUrl", item.plataformaId }
                            };
                            var response = RestHelper.ExecutePostRequest<Object>("http://apifaturamento.bemacashlocal.com.br/api/", "atualizatotaisordemvenda/atualizatotaisasync", null, null, header, 300);
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
                    var ordemVendas = unitOfWork.OrdemVendaBL.GetOrdemVendas();

                    foreach (OrdemVenda item in ordemVendas)
                    {
                        try
                        {
                            item.Total = unitOfWork.OrdemVendaBL.CalculaTotalOrdemVenda(item.Id, item.ClienteId, item.GeraNotaFiscal, item.TipoNfeComplementar.ToString(), item.TipoFrete.ToString(), item.ValorFrete == null ? 0 : item.ValorFrete, false).Total;
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