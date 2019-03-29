using Fly01.Core.API;
using Fly01.Core.Rest;
using Fly01.Financeiro.API.Models.DAL;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("relatorioLicenciamento")]
    public class RelatorioLicenciamentoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal, string plataformaUrl = "")
        {
            using (AppDataContext context = new AppDataContext())
            {
                var result = context.Database.SqlQuery<ReportVM>(
                    "SELECT * FROM GetLicenceReport(@DATAINI, @DATAFIN, @PLATAFORMAURL)",
                    new SqlParameter("DATAINI", dataInicial),
                    new SqlParameter("DATAFIN", dataFinal),
                    new SqlParameter("PLATAFORMAURL", plataformaUrl)
                ).ToList();

                var response = result.GroupBy(x => x.PlataformaUrl).Select(item => new
                {
                    PlataformaUrl = item.Key,
                    ContaReceber = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CONTASRECEBER"),
                    ContaPagar = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CONTASAPAGAR"),
                    Cliente = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CLIENTES"),
                    Fornecedor = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "FORNECEDORES"),
                    Vendedor = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "VENDEDORES"),
                    Transportadora = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "TRANSPORTADORAS"),
                    FormaPagamento = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "FORMAPAGAMENTO"),
                    CondicaoParcelamento = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CONDICAOPARCELAMENTO"),
                    Categoria = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CATEGORIA"),
                }).ToList();

                return Ok(new
                {
                    data = response.Select(i => new
                    {
                        PlataformaUrl = i.PlataformaUrl,
                        TotalContaReceber = i.ContaReceber != null ? i.ContaReceber.Total : 0,
                        TotalContaPagar = i.ContaPagar != null ? i.ContaPagar.Total : 0,
                        TotalCliente = i.Cliente != null ? i.Cliente.Total : 0,
                        TotalFornecedor = i.Fornecedor != null ? i.Fornecedor.Total : 0,
                        TotalVendedor = i.Vendedor != null ? i.Vendedor.Total : 0,
                        TotalTransportadora = i.Transportadora != null ? i.Transportadora.Total : 0,
                        TotalFormaPagamento = i.FormaPagamento != null ? i.FormaPagamento.Total : 0,
                        TotalCondicaoParcelamento = i.CondicaoParcelamento != null ? i.CondicaoParcelamento.Total : 0,
                        TotalCategoria = i.Categoria != null ? i.Categoria.Total : 0,
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