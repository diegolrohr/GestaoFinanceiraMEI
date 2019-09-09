using Fly01.Core.Presentation;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    public abstract class DashboardContaFinanceiraController : BaseController<EmpresaBaseVM>
    {
        protected string tipoConta;

        public DashboardContaFinanceiraController(string TipoConta)
        {
            tipoConta = TipoConta;
        }

        public override Func<EmpresaBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }
        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        private List<DashboardContaFinanceiraVM> GetProjecaoCategoria(DateTime dataInicial) => GetProjecao(dataInicial, "dashboardcategoria");
        private List<DashboardContaFinanceiraVM> GetProjecaoStatus(DateTime dataInicial) => GetProjecao(dataInicial, "dashboardstatus");
        private List<DashboardContaFinanceiraVM> GetProjecaoPagamento(DateTime dataInicial) => GetProjecao(dataInicial, "dashboardformapagamento");
        protected List<DashboardContaFinanceiraVM> GetProjecao(DateTime dataInicial, string resource)
        {
            const int topCount = 4;
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", tipoConta }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaFinanceiraVM>>(resource, queryString);
            if (response == null)
                return new List<DashboardContaFinanceiraVM>();
            else
            {
                if (response.Count() > topCount)
                {
                    var other = new DashboardContaFinanceiraVM
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
        protected object ChartToView(List<DashboardContaFinanceiraVM> response)
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
        protected object ChartOptions(string title = "")
        {
            return new
            {
                title = new
                {
                    display = true,
                    text = title,
                    fontSize = 15,
                    fontFamily = "Roboto",
                    fontColor = "#2c3739"
                },
                tooltips = new
                {
                    mode = "label",
                    bodySpacing = 10,
                    cornerRadius = 0,
                    titleMarginBottom = 15
                },
                legend = new
                {
                    position = "bottom"
                },
                elements = new
                {
                    center = new
                    {
                        currency = true,
                        maxText = "R$ AA.AAA,AA",
                        fontColor = "#2c3739",
                        fontFamily = "'Roboto', 'Arial', sans-serif",
                        fontStyle = "normal",
                        minFontSize = 1,
                        maxFontSize = 256,
                    }
                }
            };
        }

        public JsonResult LoadChartStatus(DateTime dataInicial) => Json(ChartToView(GetProjecaoStatus(dataInicial)), JsonRequestBehavior.AllowGet);
        public JsonResult LoadChartPagamento(DateTime dataInicial) => Json(ChartToView(GetProjecaoPagamento(dataInicial)), JsonRequestBehavior.AllowGet);
        public JsonResult LoadChartCategoria(DateTime dataInicial) => Json(ChartToView(GetProjecaoCategoria(dataInicial)), JsonRequestBehavior.AllowGet);

        public override ContentResult List() => Content(JsonConvert.SerializeObject(DashboardJson(Url, Request.Url.Scheme), JsonSerializerSetting.Front), "application/json");

        protected virtual ContentUI DashboardJson(UrlHelper url, string scheme)
        {
            if (!UserCanRead)
                return new ContentUIBase(Url.Action("Sidebar", "Home"));

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = $"Indicadores -  Contas a {tipoConta.Replace("Conta", "")}"
                },
                UrlFunctions = url.Action("Functions") + "?fns="
            };
           
            cfg.Content.Add(new FormUI
            {
                Id = "fly01frm",
                //ReadyFn = "fnUpdateData",
                Functions = new List<string> { "__format" },
                Class = "col s12",
                UrlFunctions = url.Action("Functions") + "?fns=",
                Elements = new List<BaseUI>()
                {
                    new PeriodPickerUI()
                    {
                       Label= "Selecione o período",
                       Id= "mesPicker",
                       Name= "mesPicker",
                       Class= "col s12 m4 offset-m4",
                       DomEvents = new List<DomEventUI>()
                       {
                           new DomEventUI()
                           {
                              DomEvent = "change",
                              Function = "fnUpdateData"
                           }
                       }
                    },
                    new InputHiddenUI()
                    {
                        Id = "dataInicial",
                        Name = "dataInicial",
                        Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd")
                    },
                }
            });

            // CHART Status
            cfg.Content.Add(new ChartUI
            {
                Id = "chartStatus",
                DrawType = ChartUIType.Doughnut,
                Options = ChartOptions("Status"),
                UrlData = @url.Action("LoadChartStatus"),
                Class = "col s12 m4",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });

            // CHART Pagamento
            cfg.Content.Add(new ChartUI
            {
                Id = "chartPagamento",
                DrawType = ChartUIType.Doughnut,
                Options = ChartOptions("Formas de Pagamento"),
                UrlData = @url.Action("LoadChartPagamento"),
                Class = "col s12 m4",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });

            // CHART Categoria
            cfg.Content.Add(new ChartUI
            {
                Id = "chartCategoria",
                DrawType = ChartUIType.Doughnut,
                Options = ChartOptions("Categorias"),
                UrlData = @url.Action("LoadChartCategoria"),
                Class = "col s12 m4",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });

            return cfg;
        }       
    }
}
