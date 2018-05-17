using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Rest;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Presentation.Commons;

namespace Fly01.Financeiro.Controllers
{
    public class FluxoCaixaController : Controller
    {
        public JsonResult LoadSaldos(string dataFinal)
        {
            try
            {
                if (string.IsNullOrEmpty(dataFinal))
                    dataFinal = DateTime.Now.ToString("yyyy-MM-dd");

                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataFinal", dataFinal }
                };

                var response = RestHelper.ExecuteGetRequest<ResponseFluxoCaixaSaldoVM>("fluxocaixa/saldos", queryString);
                if (response == null)
                    return Json(new FluxoCaixaSaldoVM { SaldoConsolidado = 0, APagarHoje = 0, AReceberHoje = 0 }, JsonRequestBehavior.AllowGet);

                var responseToView = new
                {
                    TotalPagamentos = response.Value.APagarHoje.ToString("C", AppDefaults.CultureInfoDefault),
                    TotalRecebimentos = response.Value.AReceberHoje.ToString("C", AppDefaults.CultureInfoDefault),
                    SaldoFinal = response.Value.SaldoConsolidado.ToString("C", AppDefaults.CultureInfoDefault)
                };

                return Json(responseToView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        //public JsonResult LoadChangeSaldos(string dataInicial, string dataFinal)
        //{
        //    try
        //    {
        //        Dictionary<string, string> queryString = new Dictionary<string, string>
        //        {
        //            { "dataInicial", dataInicial},
        //            { "dataFinal", dataFinal }
        //        };

        //        var response = RestHelper.ExecuteGetRequest<FluxoCaixaProjecaoVM>("fluxocaixa/projecaoNextDays", queryString);

        //        if (response == null)
        //            return Json(new FluxoCaixaProjecaoVM { SaldoFinal = 0, TotalPagamentos = 0, TotalRecebimentos = 0 }, JsonRequestBehavior.AllowGet);

        //        var responseToView = new
        //        {
        //            TotalPagamentos = response.TotalPagamentos.ToString("C", AppDefaults.CultureInfoDefault),
        //            TotalRecebimentos = response.TotalRecebimentos.ToString("C", AppDefaults.CultureInfoDefault),
        //            SaldoFinal = response.SaldoFinal.ToString("C", AppDefaults.CultureInfoDefault)
        //        };

        //        return Json(responseToView, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return JsonResponseStatus.GetFailure(ex.Message);
        //    }
        //}

        private List<FluxoCaixaProjecaoVM> GetProjecao(DateTime dataInicial, DateTime dataFinal)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial.ToString("yyyy-MM-dd") },
                { "dataFinal", dataFinal.ToString("yyyy-MM-dd") }
            };
            var response = RestHelper.ExecuteGetRequest<ResponseFluxoCaixaProjecaoVM>("fluxocaixa/projecao", queryString);
            if (response == null)
                return new List<FluxoCaixaProjecaoVM>();

            return response.Values;
        }

        private PagedResult<FluxoCaixaProjecaoVM> GetProjecaoDetalhe(DateTime dataInicial, DateTime dataFinal, int pageNo)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial.ToString("yyyy-MM-dd") },
                { "dataFinal", dataFinal.ToString("yyyy-MM-dd") },
                { "pageNo", pageNo.ToString() },
                { "pageSize", "10"}
            };
            var response = RestHelper.ExecuteGetRequest<PagedResult<FluxoCaixaProjecaoVM>>("fluxocaixa/projecaodetalhe", queryString);

            return response;
        }

        public JsonResult LoadChart(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                var response = GetProjecao(dataInicial, dataFinal);

                var dataChartToView = new
                {
                    success = true,
                    labels = response.Select(x => x.Data.ToString("dd/MM/yyyy")).ToArray(),
                    datasets = new object[] {
                        new {
                                type = "line",
                                label = "Saldo",
                                backgroundColor = "rgb(99, 99, 99)",
                                borderColor = "rgb(99, 99, 99)",
                                data = response.Select(x => Math.Round(x.SaldoFinal, 2)).ToArray(),
                                fill = false
                            },
                        new {
                                label = "Recebimentos",
                                fill = false,
                                backgroundColor = "rgb(75, 192, 192)",
                                borderColor = "rgb(75, 192, 192)",
                                data = response.Select(x => Math.Round(x.TotalRecebimentos, 2)).ToArray()
                            },
                        new {
                                label = "Pagamentos",
                                fill = false,
                                backgroundColor = "rgb(255, 99, 132)",
                                borderColor = "rgb(255, 99, 132)",
                                data = response.Select(x => Math.Round(x.TotalPagamentos * -1, 2)).ToArray()
                        }
                    }
                };

                return Json(dataChartToView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        public JsonResult LoadGridFluxoCaixa(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
                var pageNo = param.Start > 0 ? (param.Start / 10) + 1 : 1;

                var response = GetProjecaoDetalhe(dataInicial, dataFinal, pageNo);

                return Json(new
                {
                    recordsTotal = response.Paging.TotalRecordCount,
                    recordsFiltered = response.Paging.TotalRecordCount,
                    data = response.Data.Select(item => new
                    {
                        data = item.Data.ToString("dd/MM/yyyy"),
                        totalRecebimentos = item.TotalRecebimentos.ToString("C", AppDefaults.CultureInfoDefault),
                        totalPagamentos = (item.TotalPagamentos * -1).ToString("C", AppDefaults.CultureInfoDefault),
                        saldoFinal = item.SaldoFinal.ToString("C", AppDefaults.CultureInfoDefault)
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }
    }
}