using Fly01.Core.API;
using Fly01.Core.Rest;
using Fly01.Faturamento.DAL;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("relatorioLicenciamentoFaturamento")]
    public class RelatorioLicenciamentoFaturamentoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime? dataInicial, DateTime? dataFinal, string plataformaUrl)
        {
            using (AppDataContext context = new AppDataContext())
            {
                var result = context.Database.SqlQuery<ReportVM>(
                    "SELECT * FROM GetFaturamentoReport(@DATAINI, @DATAFIN, @PLATAFORMAURL)",
                    new SqlParameter("DATAINI", dataInicial.HasValue ? dataInicial.Value.ToString("yyyy-MM-dd") : ""),
                    new SqlParameter("DATAFIN", dataFinal.HasValue ? dataFinal.Value.ToString("yyyy-MM-dd") : ""),
                    new SqlParameter("PLATAFORMAURL", plataformaUrl ?? "")
                ).ToList();

                var response = result.GroupBy(x => x.PlataformaUrl).Select(item => new
                {
                    PlataformaUrl = item.Key,
                    Venda = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "VENDAS"),
                    Orcamento = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "ORCAMENTOS"),
                    NFe = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "NFE"),
                    NFSe = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "NFSE"),
                    Cliente = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CLIENTE"),
                    Fornecedor = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "FORNECEDOR"),
                    GrupoTributario = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "GRUPOTRIBUTARIO"),
                    Produto = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "PRODUTOS"),
                    Servico = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "SERVICOS"),
                    Kit = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "KITS"),
                    CentroCusto = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CENTROCUSTO"),
                    CertificadoDigital = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CERTIFICADODIGITAL")
                }).ToList();

                return Ok(new
                {
                    data = response.Select(i => new
                    {
                        PlataformaUrl = i.PlataformaUrl,
                        Venda = i.Venda != null ? i.Venda.Total : 0,
                        Orcamento = i.Orcamento != null ? i.Orcamento.Total : 0,
                        NFe = i.NFe != null ? i.NFe.Total : 0,
                        NFSe = i.NFSe != null ? i.NFSe.Total : 0,
                        Cliente = i.Cliente != null ? i.Cliente.Total : 0,
                        Fornecedor = i.Fornecedor != null ? i.Fornecedor.Total : 0,
                        GrupoTributario = i.GrupoTributario != null ? i.GrupoTributario.Total : 0,
                        Produto = i.Produto != null ? i.Produto.Total : 0,
                        Servico = i.Servico != null ? i.Servico.Total : 0,
                        Kit = i.Kit != null ? i.Kit.Total : 0,
                        CentroCusto = i.CentroCusto != null ? i.CentroCusto.Total : 0,
                        CertificadoDigital = i.CertificadoDigital != null ? "Sim" : "Não",
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