using Fly01.Core.API;
using Fly01.Core.Rest;
using Fly01.OrdemServico.DAL;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    [RoutePrefix("relatorioLicenciamentoOS")]
    public class RelatorioLicenciamentoOSController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime? dataInicial, DateTime? dataFinal, string plataformaUrl)
        {
            using (AppDataContext context = new AppDataContext())
            {
                var result = context.Database.SqlQuery<ReportVM>(
                    "SELECT * FROM GetOSReport(@DATAINI, @DATAFIN, @PLATAFORMAURL)",
                    new SqlParameter("DATAINI", dataInicial.HasValue ? dataInicial.Value.ToString("yyyy-MM-dd") : ""),
                    new SqlParameter("DATAFIN", dataFinal.HasValue ? dataFinal.Value.ToString("yyyy-MM-dd") : ""),
                    new SqlParameter("PLATAFORMAURL", plataformaUrl ?? "")
                ).ToList();

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
                        RazaoSocial = ApiEmpresaManager.GetEmpresa(i.PlataformaUrl).RazaoSocial
                    }).ToList()
                });
            }
        }
    }

    public class ReportVM
    {
        public string PlataformaUrl { get; set; }
        public string Tipo { get; set; }
        public int Total { get; set; }
    }
}