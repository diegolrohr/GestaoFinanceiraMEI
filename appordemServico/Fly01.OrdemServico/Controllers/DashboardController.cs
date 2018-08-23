using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasDashboard)]
    public class DashboardController : BaseController<DomainBaseVM>
    {

        private List<OrdemServicosPorDiaVM> GetProjecao(DateTime filtro)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", filtro.ToString("yyyy-MM-dd") }
            };
            var response = RestHelper.ExecuteGetRequest<ResponseOrdemServicosPorDiaVM>("dashboard/quantidadeordemservicopordia", queryString);
            if (response == null)
                return new List<OrdemServicosPorDiaVM>();

            return response.Values;
        }

        public JsonResult LoadChart(DateTime filtro)
        {
            try
            {
                var response = GetProjecao(filtro);

                var dataChartToView = new
                {
                    success = true,
                    currency = true,
                    //labels = response.Select(x => x.Label).ToArray(),
                    datasets = new object[] {
                        new {
                                type = "line",
                                label = "Saldo",
                                backgroundColor = "rgb(44, 55, 57)",
                                borderColor = "rgb(44, 55, 57)",
                                //data = response.Select(x => Math.Round(x.SaldoFinal, 2)).ToArray(),
                                fill = false
                            },
                        new {
                                label = "Recebimentos",
                                fill = false,
                                backgroundColor = "rgb(0, 178, 121)",
                                borderColor = "rgb(0, 178, 121)",
                                //data = response.Select(x => Math.Round(x.TotalRecebimentos, 2)).ToArray()
                            },
                        new {
                                label = "Pagamentos",
                                fill = false,
                                backgroundColor = "rgb(239, 100, 97)",
                                borderColor = "rgb(239, 100, 97)",
                                //data = response.Select(x => Math.Round(x.TotalPagamentos * -1, 2)).ToArray()
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

        public JsonResult LoadCards(string dataFinal)
        {
            try
            {
                if (string.IsNullOrEmpty(dataFinal))
                    dataFinal = DateTime.Now.ToString("yyyy-MM-dd");

                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "filtro", dataFinal }
                };
                var response = RestHelper.ExecuteGetRequest<OrdemServicoPorStatusVM[]>("dashboard/status", queryString);
                var aberto = "0/0";
                var andamento = "0/0";
                var concluido = "0/0";
                var cancelado = "0/0";

                foreach (var item in response)
                {
                    if (item.Status == "Em Aberto") aberto = item.Quantidade.ToString() + "/" + item.QuantidadeTotal.ToString();
                    else if (item.Status == "Em Andamento") andamento = item.Quantidade.ToString() + "/" + item.QuantidadeTotal.ToString();
                    else if (item.Status == "Concluído") concluido = item.Quantidade.ToString() + "/" + item.QuantidadeTotal.ToString();
                    else if (item.Status == "Cancelado") cancelado = item.Quantidade.ToString() + "/" + item.QuantidadeTotal.ToString();
                }
                var responseToView = new
                {
                    EmAberto = aberto,
                    EmAndamento = andamento,
                    Concluido = concluido,
                    Cancelado = cancelado
                };

                return Json(responseToView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        public JsonResult DashboardTopProdutos(DateTime? dataInicial = null)
        {
            try
            {
                Dictionary<string, string> querystring = new Dictionary<string, string>
                    {
                        { "filtro", dataInicial.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") }
                    };

                var response = RestHelper.ExecuteGetRequest<List<TopServicosProdutosOrdemServicoVM>>("dashboard/topprodutosordemservico", querystring);

                return Json(new
                {
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count,
                    data = response.Select(x => new
                    {
                        descricao = x.Descricao,
                        valor = 0, //x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                        quantidade = x.Quantidade,
                        valorTotal = x.ValorTotal
                    })
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult DashboardTopServicos(DateTime? dataInicial = null)
        {
            try
            {
                Dictionary<string, string> querystring = new Dictionary<string, string>
                    {
                        { "filtro", dataInicial.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") }
                    };

                var response = RestHelper.ExecuteGetRequest<List<TopServicosProdutosOrdemServicoVM>>("dashboard/topservicosordemservico", querystring);

                return Json(new
                {
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count,
                    data = response.Select(x => new
                    {
                        descricao = x.Descricao,
                        valor = 0, //x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                        quantidade = x.Quantidade,
                        valorTotal = x.ValorTotal
                    })
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
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
    }
}
