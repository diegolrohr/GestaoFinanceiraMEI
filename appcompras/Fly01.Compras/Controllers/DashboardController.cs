using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasDashboard)]
    public class DashboardController : BaseController<DomainBaseVM>
    {
        protected object ChartToView(List<DashboardComprasVM> response)
        {
            var colors = new[]
            {
                "rgba(250, 166, 52, 0.9)",
                "rgba(243, 112, 33, 0.9)",
                "rgba(0, 52, 88, 0.9)",
                "rgba(0, 103, 139, 0.9)",
                "rgba(12, 154, 190, 0.9)",
            };
            return new
            {
                success = true,
                currency = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[]
                {
                    new
                    {
                        data = response.Select(x => Math.Round(x.Total, 2)).ToArray(),
                        backgroundColor = colors,
                        borderWidth = 1
                    }
                }
            };
        }

        public JsonResult Status(DateTime dataInicial, string tpOrdemCompra) => Json(ChartToView(GetProjecaoStatus(dataInicial, tpOrdemCompra)), JsonRequestBehavior.AllowGet);
        public JsonResult Categoria(DateTime dataInicial, string tpOrdemCompra) => Json(ChartToView(GetProjecaoCategoria(dataInicial, tpOrdemCompra)), JsonRequestBehavior.AllowGet);
        public JsonResult FormaPagamento(DateTime dataInicial, string tpOrdemCompra) => Json(ChartToView(GetProjecaoFormaPagamento(dataInicial, tpOrdemCompra)), JsonRequestBehavior.AllowGet);

        public JsonResult MaioresFornecedores(DateTime? dataInicial = null)
        {
            try
            {
                Dictionary<string, string> querystring = new Dictionary<string, string>
                    {
                        { "filtro", dataInicial.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") }
                    };

                var response = RestHelper.ExecuteGetRequest<List<MaioresFornecedoresVM>>("dashboard/maioresfornecedores", querystring);

                return Json(new
                {
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count,
                    data = response.Select(x => new
                    {
                        nome = x.Nome,
                        valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                    })
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }
        public JsonResult MaisComprados(DateTime? dataInicial = null)
        {
            try
            {
                Dictionary<string, string> querystring = new Dictionary<string, string>
                    {
                        { "filtro", dataInicial.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") }
                    };

                var response = RestHelper.ExecuteGetRequest<List<ProdutosMaisCompradosVM>>("dashboard/produtosmaiscomprados", querystring);

                return Json(new
                {
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count,
                    data = response.Select(x => new
                    {
                        descricao = x.Descricao,
                        valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                        quantidade = x.Quantidade,
                        valorTotal = (x.Quantidade * x.Valor).ToString("C", AppDefaults.CultureInfoDefault)
                    })
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private List<DashboardComprasVM> GetProjecaoStatus(DateTime dataInicial, string tpOrdemCompra) => GetProjecao(dataInicial, tpOrdemCompra, "dashboard/status");
        private List<DashboardComprasVM> GetProjecaoCategoria(DateTime dataInicial, string tpOrdemCompra) => GetProjecao(dataInicial, tpOrdemCompra, "dashboard/categoria");
        private List<DashboardComprasVM> GetProjecaoFormaPagamento(DateTime dataInicial, string tpOrdemCompra) => GetProjecao(dataInicial, tpOrdemCompra, "dashboard/formapagamento");
        protected List<DashboardComprasVM> GetProjecao(DateTime dataInicial, string tpOrdemCompra, string resource)
        {
            const int topCount = 4;
            Dictionary<string, string> querystring = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", tpOrdemCompra }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardComprasVM>>(resource, querystring);
            if (response == null)
                return new List<DashboardComprasVM>();
            else
            {
                if (response.Count() > topCount)
                {
                    var other = new DashboardComprasVM
                    {
                        Tipo = "Outras",
                        Total = response.OrderByDescending(x => x.Total).Skip(topCount).Sum(x => x.Total)
                    };

                    response = response.OrderByDescending(x => x.Total).Take(topCount).ToList();
                    response.Add(other);
                }
            }
            return response;
        }

        public override Func<DomainBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }
        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}
