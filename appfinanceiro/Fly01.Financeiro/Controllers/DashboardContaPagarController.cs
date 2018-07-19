using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
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
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroDashboardContasPagar)]
    public class DashboardContaPagarController : BaseController<DashboardContaPagarVM>
    {
        private List<DashboardContaPagarChartVM> GetProjecaoCategoria(DateTime dataInicial) => GetProjecao(dataInicial, "dashboardcategoria");
        private List<DashboardContaPagarChartVM> GetProjecaoStatus(DateTime dataInicial) => GetProjecao(dataInicial, "dashboardstatus");
        private List<DashboardContaPagarChartVM> GetProjecaoPagamento(DateTime dataInicial) => GetProjecao(dataInicial, "dashboardformapagamento");
        private List<DashboardContaPagarChartVM> GetProjecao(DateTime dataInicial, string resource)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", "ContaPagar" }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaPagarChartVM>>(resource, queryString);
            if (response == null)
                return new List<DashboardContaPagarChartVM>();
            else
            {
                if (response.Count() > 5)
                {
                    var other = new DashboardContaPagarChartVM
                    {
                        Tipo = "Outras",
                        Total = response.OrderByDescending(x => x.Total).Skip(5).Sum(x => x.Total),
                        Quantidade = response.OrderByDescending(x => x.Total).Skip(5).Sum(x => x.Quantidade)
                    };

                    response = response.OrderByDescending(x => x.Total).Take(5).ToList();
                    response.Add(other);
                }
                
            }               

            return response;
        }
        private object ChartToView(List<DashboardContaPagarChartVM> response)
        {
            var colors = new[]
            {
                "rgba(243, 112, 33, 0.9)",
                "rgba(250, 166, 52, 0.9)",
                "rgba(0, 52, 88, 0.9)",
                "rgba(0, 103, 139, 0.9)",
                "rgba(12, 154, 190, 0.9)",
            };
            return new
            {
                success = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[]
                {
                    new
                    {
                        data = response.Select(x => Math.Round(x.Total, 2)).ToArray(),
                        backgroundColor = colors,
                        borderWidth = 1
                    },
                    new
                    {
                        data = response.Select(x => (x.Quantidade)).ToArray(),
                        backgroundColor = colors,
                        borderWidth = 1
                    }

                }
            };
        }
        private object ChartOptions(string title = "")
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
                global = new
                {
                    responsive = true,
                    maintainAspectRatio = true
                },
                elements = new
                {
                    center = new
                    {
                        maxText= "AAAAAA",
					    fontColor= "#2c3739",
                        fontFamily = "'Roboto', 'Arial', sans-serif",
					    fontStyle= "normal",
                        minFontSize = 1,
					    maxFontSize= 256,
				    }
                }
            };
        }

        public JsonResult LoadChartStatus(DateTime dataInicial) => Json(ChartToView(GetProjecaoStatus(dataInicial)), JsonRequestBehavior.AllowGet);
        public JsonResult LoadChartPagamento(DateTime dataInicial) => Json(ChartToView(GetProjecaoPagamento(dataInicial)), JsonRequestBehavior.AllowGet);
        public JsonResult LoadChartCategoria(DateTime dataInicial) => Json(ChartToView(GetProjecaoCategoria(dataInicial)), JsonRequestBehavior.AllowGet);

        public override Func<DashboardContaPagarVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }
        public override ContentResult List()
            => Content(JsonConvert.SerializeObject(DashboardContaPagarJson(Url, Request.Url.Scheme), JsonSerializerSetting.Front), "application/json");
        public JsonResult DashboardGridLoad(DateTime? dataInicial = null)
        {
            try
            {
                var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);

                var pageNo = param.Start > 0 ? (param.Start / 10) + 1 : 1;

                Dictionary<string, string> queryString = new Dictionary<string, string>
                    {
                        { "filtro", dataInicial.HasValue? dataInicial.Value.ToString("yyyy-MM-dd"):DateTime.Now.ToString("yyyy-MM-dd") },
                        { "pageNo", pageNo.ToString() },
                        { "pageSize", "10" }
                    };

                var response = RestHelper.ExecuteGetRequest<PagedResult<ContasaPagarDoDiaVM>>("dashboardcontapagardia", queryString);

                return Json(new
                {
                    totalRecords = response.Paging.TotalRecordCount,
                    recordsFiltered = response.Paging.TotalRecordCount,
                    data = response.Data.Select(x => new
                    {
                        vencimento = x.Vencimento,
                        descricao = x.Descrição,
                        valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                        status = x.Status
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected ContentUI DashboardContaPagarJson(UrlHelper url, string scheme, bool withSidebarUrl = false)
        {
            if (!UserCanRead)
                return new ContentUI();

            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = url.Action("Index", "DashboardContaPagar") },
                Header = new HtmlUIHeader
                {
                    Title = "Dashboard -  Contas a Pagar"
                },
                UrlFunctions = url.Action("Functions", "DashboardContaPagar") + "?fns="
            };

            if (withSidebarUrl)
                cfg.SidebarUrl = url.Action("Sidebar", "DashboardContaPagar", null, scheme);

            cfg.Content.Add(new FormUI
            {
                ReadyFn = "fnFormReady",
                Functions = new List<string> { "__format" },
                Class = "col s12",
                UrlFunctions = url.Action("Functions", "DashboardContaPagar") + "?fns=",
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
                        Name = "dataInicial"
                    },
                }
            });

            // CHART Status
            cfg.Content.Add(new ChartUI
            {
                Id = "chartStatus",
                DrawType = ChartUIType.Doughnut,
                Options = ChartOptions("Status"),
                UrlData = @url.Action("LoadChartStatus", "DashboardContaPagar"),
                Class = "col s12 l4",
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
                UrlData = @url.Action("LoadChartPagamento", "DashboardContaPagar"),
                Class = "col s12 l4",
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
                UrlData = @url.Action("LoadChartCategoria", "DashboardContaPagar"),
                Class = "col s12 l4",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });

            // Grid
            cfg.Content.Add(new DivUI
            {
                Id = "divLabel",
                Elements = new List<BaseUI> {
                    new LabelSetUI {
                        Id = "titleLabel",
                        Class = "col s12",
                        Label = "Contas a Pagar - Dia"
                    }
                }
            });
            cfg.Content.Add(new DataTableUI
            {
                Id = "dtDashCP",
                UrlGridLoad = url.Action("DashboardGridLoad"),
                Parameters = new List<DataTableUIParameter>
                    {
                        new DataTableUIParameter { Id = "dataInicial", Required = true }
                    },
                Options = new DataTableUIConfig()
                {
                    PageLength = 10,
                    WithoutRowMenu = true
                },
                UrlFunctions = url.Action("Functions", "DashboardContaPagar", null) + "?fns=",
                Columns = new List<DataTableUIColumn> {
                    new DataTableUIColumn
                    {
                        DataField = "descricao",
                        DisplayName = "Descrição",
                        Priority = 1,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "vencimento",
                        DisplayName = "Vencimento",
                        Priority = 2,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "status",
                        DisplayName = "Status",
                        Priority = 3,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "valor",
                        DisplayName = "Valor",
                        Priority = 4,
                        Type = "valor",
                        Orderable = false,
                        Searchable = false
                    }
                }
            });

            return cfg;
        }
        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
    }
}
