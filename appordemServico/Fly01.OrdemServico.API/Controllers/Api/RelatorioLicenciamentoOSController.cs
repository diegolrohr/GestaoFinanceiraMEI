using Fly01.Core.API;
using Fly01.Core.Rest;
using Fly01.OrdemServico.DAL;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [RoutePrefix("relatorioLicenciamentoOS")]
    public class RelatorioLicenciamentoOSController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(object model)
        {
            var requestParams = JsonConvert.DeserializeObject<RequestParamsVM>(JsonConvert.SerializeObject(model));
            using (AppDataContext context = new AppDataContext())
            {
                var result = context.Database.SqlQuery<ReportVM>(
                    string.Format("SELECT * FROM GetOSReport('{0}', '{1}', '{2}')",
                    requestParams.DataInicial.HasValue ? requestParams.DataInicial.Value.ToString("yyyy-MM-dd") : "",
                    requestParams.DataFinal.HasValue ? requestParams.DataFinal.Value.ToString("yyyy-MM-dd") : "",
                    requestParams.PlataformaUrl ?? ""
                    )).ToList();

                var response = result.GroupBy(x => x.PlataformaUrl).Select(item => new
                {
                    PlataformaUrl = item.Key,
                    OrdemServico = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "ORDEMSERVICO"),
                    Produto = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "PRODUTOS"),
                    Servico = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "SERVICOS"),
                    Cliente = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CLIENTE"),
                    Kit = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "KITS"),
                }).ToList();

                return Ok(new
                {
                    data = response.Select(i => new
                    {
                        PlataformaUrl = i.PlataformaUrl,
                        OrdemServico = i.OrdemServico != null ? i.OrdemServico.Total : 0,
                        Produto = i.Produto != null ? i.Produto.Total : 0,
                        Servico = i.Servico != null ? i.Servico.Total : 0,
                        Cliente = i.Cliente != null ? i.Cliente.Total : 0,
                        Kit = i.Kit != null ? i.Kit.Total : 0,
                        RazaoSocial = ""
                    }).ToList()
                });
            }
        }
    }

    public class RequestParamsVM
    {
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public string PlataformaUrl { get; set; }
    }

    public class ReportVM
    {
        public string PlataformaUrl { get; set; }
        public string Tipo { get; set; }
        public int Total { get; set; }
    }
}