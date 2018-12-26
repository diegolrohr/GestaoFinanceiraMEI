using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Financeiro.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(NotApply = true)]
    public class FluxoCaixaController : PrimitiveBaseController
    {
        public JsonResult LoadSaldos(string dataFinal, string dataInicial)
        {
            try
            {
                if (string.IsNullOrEmpty(dataFinal))
                    dataFinal = DateTime.Now.ToString("yyyy-MM-dd");

                if (string.IsNullOrEmpty(dataInicial))
                    dataInicial = DateTime.Now.ToString("yyyy-MM-dd");

                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataFinal", dataFinal },
                    { "dataInicial", dataInicial },
                };

                var response = RestHelper.ExecuteGetRequest<ResponseFluxoCaixaSaldoVM>("fluxocaixa/saldos", queryString);
               
                var responseToView = new
                {
                    TotalPagamentos = response.Value.TotalPagamentos.ToString("C", AppDefaults.CultureInfoDefault),
                    TotalRecebimentos = response.Value.TotalRecebimentos.ToString("C", AppDefaults.CultureInfoDefault),
                    SaldoAtual = response.Value.SaldoAtual.ToString("C", AppDefaults.CultureInfoDefault),
                    SaldoProjetado = response.Value.SaldoProjetado.ToString("C", AppDefaults.CultureInfoDefault)
                };

                return Json(responseToView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        private List<FluxoCaixaProjecaoVM> GetProjecao(DateTime dataInicial, DateTime dataFinal, int groupType)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial.ToString("yyyy-MM-dd") },
                { "dataFinal", dataFinal.ToString("yyyy-MM-dd") },
                { "groupType", groupType.ToString() }
            };
            var response = RestHelper.ExecuteGetRequest<ResponseFluxoCaixaProjecaoVM>("fluxocaixa/projecao", queryString);
            if (response == null)
                return new List<FluxoCaixaProjecaoVM>();

            return response.Values;
        }

        private PagedResult<FluxoCaixaProjecaoVM> GetProjecaoDetalhe(DateTime dataInicial, DateTime dataFinal, int groupType, int pageNo, int length)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial.ToString("yyyy-MM-dd") },
                { "dataFinal", dataFinal.ToString("yyyy-MM-dd") },
                { "groupType", groupType.ToString() },
                { "pageNo", pageNo.ToString() },
                { "pageSize", length.ToString()}
            };
            var response = RestHelper.ExecuteGetRequest<PagedResult<FluxoCaixaProjecaoVM>>("fluxocaixa/projecaodetalhe", queryString);

            return response;
        }

        public JsonResult LoadChart(DateTime dataInicial, DateTime dataFinal, int groupType)
        {
            try
            {
                var response = GetProjecao(dataInicial, dataFinal, groupType);

                var dataChartToView = new
                {
                    success = true,
                    currency = true,
                    labels = response.Select(x => x.Label).ToArray(),
                    datasets = new object[] {
                        new {
                                type = "line",
                                label = "Saldo",
                                backgroundColor = "rgb(44, 55, 57)",
                                borderColor = "rgb(44, 55, 57)",
                                data = response.Select(x => Math.Round(x.SaldoFinal, 2)).ToArray(),
                                fill = false
                            },
                        new {
                                label = "Recebimentos",
                                fill = false,
                                backgroundColor = "rgb(0, 178, 121)",
                                borderColor = "rgb(0, 178, 121)",
                                data = response.Select(x => Math.Round(x.TotalRecebimentos, 2)).ToArray()
                            },
                        new {
                                label = "Pagamentos",
                                fill = false,
                                backgroundColor = "rgb(239, 100, 97)",
                                borderColor = "rgb(239, 100, 97)",
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

        public JsonResult LoadGridFluxoCaixa(DateTime dataInicial, DateTime dataFinal, int groupType, int length)
        {
            try
            {
                var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
                var pageNo = param.Start > 0 ? (param.Start / length) + 1 : 1;

                var response = GetProjecaoDetalhe(dataInicial, dataFinal, groupType, pageNo, length);

                return Json(new
                {
                    recordsTotal = response.Paging.TotalRecordCount,
                    recordsFiltered = response.Paging.TotalRecordCount,
                    data = response.Data.Select(item => new
                    {
                        data = item.Label,
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