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

namespace Fly01.OrdemServico.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.OrdemServicoVisaoGeral)]
    public class DashboardController : BaseController<DomainBaseVM>
    {

        private List<OrdemServicosPorDiaVM> GetProjecao(DateTime? filtro=null)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", filtro.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd")}
            };
            var response = RestHelper.ExecuteGetRequest<ResponseOrdemServicosPorDiaVM>("dashboard/quantidadeordemservicopordia", queryString);
            if (response == null)
                return new List<OrdemServicosPorDiaVM>();

            return response.Values;
        }

        public JsonResult LoadChart(DateTime? dataFinal=null)
        {
            try
            {
                var response = GetProjecao(dataFinal);

                var dataChartToView = new
                {
                    success = true,
                    currency = false,                    
                    labels = response.Select(x =>  "Dia " + x.Label).ToArray(),
                    datasets = new object[] {
                        new {
                                type = "line",
                                label = "Quantidade de OS: ",
                                backgroundColor = "rgb(44, 55, 57)",
                                borderColor = "rgb(44, 55, 57)",
                                data = response.Select(x => x.QuantidadeServicos).ToArray(),
                                fill = false
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

        public JsonResult LoadCards(DateTime? dataFinal = null, DateTime? dataInicial = null)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataFinal", dataFinal.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") },
                    { "dataInicial", dataInicial.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") }
                };
                var response = RestHelper.ExecuteGetRequest<OrdemServicoPorStatusVM[]>("dashboard/status", queryString);
                var qtdTotal = 0;
                var aberto = "0/0";
                var andamento = "0/0";
                var concluido = "0/0";
                var cancelado = "0/0";

                if (response.Any())
                {
                    qtdTotal = response.First().QuantidadeTotal;
                    aberto = "0/" + qtdTotal;
                    andamento = "0/" + qtdTotal;
                    concluido = "0/" + qtdTotal;
                    cancelado = "0/" + qtdTotal;
                }


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

        public JsonResult DashboardTopProdutos(DateTime? dataFinal = null)
        {
            try
            {
                Dictionary<string, string> querystring = new Dictionary<string, string>
                    {
                        { "filtro", dataFinal.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") }
                    };

                var response = RestHelper.ExecuteGetRequest<List<TopServicosProdutosOrdemServicoVM>>("dashboard/topprodutosordemservico", querystring);

                return Json(new
                {
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count,
                    data = response.Select(x => new
                    {
                        descricao = x.Descricao,
                        quantidade = x.Quantidade,
                        valorTotal = x.ValorTotal.ToString("C", AppDefaults.CultureInfoDefault)
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult DashboardTopServicos(DateTime? dataFinal = null)
        {
            try
            {
                Dictionary<string, string> querystring = new Dictionary<string, string>
                    {
                        { "filtro", dataFinal.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") }
                    };

                var response = RestHelper.ExecuteGetRequest<List<TopServicosProdutosOrdemServicoVM>>("dashboard/topservicosordemservico", querystring);

                return Json(new
                {
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count,
                    data = response.Select(x => new
                    {
                        descricao = x.Descricao,
                        quantidade = x.Quantidade,
                        valorTotal = x.ValorTotal.ToString("C", AppDefaults.CultureInfoDefault)
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

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}
