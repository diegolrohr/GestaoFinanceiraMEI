using Fly01.Core.API;
using Fly01.Estoque.DAL;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("RelatorioLicenciamentoEstoque")]
    public class RelatorioLicenciamentoEstoqueController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(object model)
        {
            var requestParams = JsonConvert.DeserializeObject<RequestParamsVM>(JsonConvert.SerializeObject(model));

            using (AppDataContext context = new AppDataContext())
            {
                var result = context.Database.SqlQuery<ReportVM>(
                    string.Format("SELECT * FROM GetEstoqueReport('{0}', '{1}', '{2}')",
                    requestParams.DataInicial.HasValue ? requestParams.DataInicial.Value.ToString("yyyy-MM-dd") : "",
                    requestParams.DataFinal.HasValue ? requestParams.DataFinal.Value.ToString("yyyy-MM-dd") : "",
                    requestParams.PlataformaUrl ?? ""
                    )).ToList();

                var response = result.GroupBy(x => x.PlataformaUrl).Select(item => new
                {
                    PlataformaUrl = item.Key,
                    Inventario = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "INVENTARIOS"),
                    Movimentacao = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "MOVIMENTACOES"),
                    Produto = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "PRODUTOS"),
                    GrupoProduto = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "GRUPOS PRODUTO"),
                    TipoMovimento = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "TIPOS MOVIMENTOS"),
                }).ToList();

                return Ok(new
                {
                    success = true,
                    data = response.Select(i => new
                    {
                        PlataformaUrl = i.PlataformaUrl,
                        Inventario = i.Inventario != null ? i.Inventario.Total : 0,
                        Movimentacao = i.Movimentacao != null ? i.Movimentacao.Total : 0,
                        Produto = i.Produto != null ? i.Produto.Total : 0,
                        GrupoProduto = i.GrupoProduto != null ? i.GrupoProduto.Total : 0,
                        TipoMovimento = i.TipoMovimento != null ? i.TipoMovimento.Total : 0,
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