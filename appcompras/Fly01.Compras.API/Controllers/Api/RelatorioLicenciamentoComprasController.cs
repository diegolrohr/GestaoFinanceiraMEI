using Fly01.Core.API;
using Fly01.Compras.DAL;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Http;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("relatoriolicenciamentocompras")]
    public class RelatorioLicenciamentoComprasController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(RequestParamsVM model)
        {
            using (AppDataContext context = new AppDataContext())
            {
                var result = context.Database.SqlQuery<ReportVM>(
                    string.Format("SELECT * FROM GetComprasReport('{0}', '{1}', '{2}')",
                    model.DataInicial.HasValue ? model.DataInicial.Value.ToString("yyyy-MM-dd") : "",
                    model.DataFinal.HasValue ? model.DataFinal.Value.ToString("yyyy-MM-dd") : "",
                    model.PlataformaUrl ?? ""
                    )).ToList();

                var response = result.GroupBy(x => x.PlataformaUrl).Select(item => new
                {
                    PlataformaUrl = item.Key,
                    Pedido = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "PEDIDOS"),
                    Orcamento = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "ORCAMENTOS"),
                    NFe = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "NFE"),
                    ImportacaoXML = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "IMPORTACAOXML"),
                    Cliente = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CLIENTES"),
                    Fornecedor = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "FORNECEDORES"),
                    GrupoTributario = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "GRUPOTRIBUTARIO"),
                    Produto = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "PRODUTOS"),
                    Servico = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "SERVICOS"),
                    Kit = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "KITS"),
                    CentroCusto = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CENTROCUSTO"),
                    CertificadoDigital = item.FirstOrDefault(i => i.PlataformaUrl == item.Key && i.Tipo == "CERTIFICADODIGITAL")
                }).ToList();

                return Ok(new
                {
                    success = true,
                    data = response.Select(i => new
                    {
                        PlataformaUrl = i.PlataformaUrl,
                        Pedido = i.Pedido != null ? i.Pedido.Total : 0,
                        Orcamento = i.Orcamento != null ? i.Orcamento.Total : 0,
                        NFe = i.NFe != null ? i.NFe.Total : 0,
                        ImportacaoXML = i.ImportacaoXML != null ? i.ImportacaoXML.Total : 0,
                        Cliente = i.Cliente != null ? i.Cliente.Total : 0,
                        Fornecedor = i.Fornecedor != null ? i.Fornecedor.Total : 0,
                        GrupoTributario = i.GrupoTributario != null ? i.GrupoTributario.Total : 0,
                        Produto = i.Produto != null ? i.Produto.Total : 0,
                        Servico = i.Servico != null ? i.Servico.Total : 0,
                        Kit = i.Kit != null ? i.Kit.Total : 0,
                        CentroCusto = i.CentroCusto != null ? i.CentroCusto.Total : 0,
                        CertificadoDigital = i.CertificadoDigital != null ? "Sim" : "Não"
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