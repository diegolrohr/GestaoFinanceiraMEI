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
        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override Func<DashboardContaPagarVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
            => Content(JsonConvert.SerializeObject(DashboardContaPagarJson(Url, Request.Url.Scheme), JsonSerializerSetting.Front), "application/json");

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
                    new InputHiddenUI()
                    {
                        Id = "dataFinal",
                        Name = "dataFinal"
                    },
                    new ButtonGroupUI
                    {
                        Id = "fly01btngrp",
                        Class = "col s12 m8 offset-m2",
                        OnClickFn = "fnAtualizarChart",
                        Options = new List<ButtonGroupOptionUI>
                        {
                            new ButtonGroupOptionUI {Id = "btnStatus", Value = "1", Label = "Status", Class="col s3"},
                            new ButtonGroupOptionUI {Id = "btnFormaPagamento", Value = "2", Label = "Forma de Pagamento", Class="col s6"},
                            new ButtonGroupOptionUI {Id = "btnCategoria", Value = "3", Label = "Categoria", Class="col s3" }
                        }
                    }
                }
            });

            // CHART Status
            cfg.Content.Add(new ChartUI
            {
                Id = "chartStatusVlr",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Valor - Total",
                        fontSize = 15,
                        fontFamily = "Roboto",
                        fontColor = "#555"
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
                        responsive = false,
                        maintainAspectRatio = false
                    },
                    scales = new
                    {
                        xAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            },
                        yAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            }
                    }
                },
                UrlData = @url.Action("LoadChartStatusVlr", "DashboardContaPagar"),
                Class = "col s12 m6 l6",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });
            cfg.Content.Add(new ChartUI
            {
                Id = "chartStatusQtd",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Quantidade - Total",
                        fontSize = 15,
                        fontFamily = "Roboto",
                        fontColor = "#555"
                    },
                    tooltips = new
                    {
                        mode = "label",
                        bodySpacing = 10,
                        cornerRadius = 0,
                        titleMarginBottom = 15
                    },
                    legend = new { position = "bottom" },
                    global = new
                    {
                        responsive = false,
                        maintainAspectRatio = false
                    },
                    scales = new
                    {
                        xAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            },
                        yAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            }
                    }
                },
                UrlData = @url.Action("LoadChartStatusQtd", "DashboardContaPagar"),
                Class = "col s12 m6 l6",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });

            // CHART Pagamento
            cfg.Content.Add(new ChartUI
            {
                Id = "chartPagamentoVlr",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Valor - Total",
                        fontSize = 15,
                        fontFamily = "Roboto",
                        fontColor = "#555"
                    },
                    tooltips = new
                    {
                        mode = "label",
                        bodySpacing = 10,
                        cornerRadius = 0,
                        titleMarginBottom = 15
                    },
                    legend = new { position = "bottom" },
                    global = new
                    {
                        responsive = false,
                        maintainAspectRatio = false
                    },
                    scales = new
                    {
                        xAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            },
                        yAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            }
                    }
                },
                UrlData = @url.Action("LoadChartPagamentoVlr", "DashboardContaPagar"),
                Class = "col s12 m6 l6",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });
            cfg.Content.Add(new ChartUI
            {
                Id = "chartPagamentoQtd",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Quantidade - Total",
                        fontSize = 15,
                        fontFamily = "Roboto",
                        fontColor = "#555"
                    },
                    tooltips = new
                    {
                        mode = "label",
                        bodySpacing = 10,
                        cornerRadius = 0,
                        titleMarginBottom = 15
                    },
                    legend = new { position = "bottom" },
                    global = new
                    {
                        responsive = false,
                        maintainAspectRatio = false
                    },
                    scales = new
                    {
                        xAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            },
                        yAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            }
                    }
                },
                UrlData = @url.Action("LoadChartPagamentoQtd", "DashboardContaPagar"),
                Class = "col s12 m6 l6",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });

            // CHART Categoria
            cfg.Content.Add(new ChartUI
            {
                Id = "chartCategoriaVlr",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Valor - Total",
                        fontSize = 15,
                        fontFamily = "Roboto",
                        fontColor = "#555"
                    },
                    tooltips = new
                    {
                        mode = "label",
                        bodySpacing = 10,
                        cornerRadius = 0,
                        titleMarginBottom = 15
                    },
                    legend = new { position = "bottom" },
                    global = new
                    {
                        responsive = false,
                        maintainAspectRatio = false
                    },
                    scales = new
                    {
                        xAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            },
                        yAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            }
                    }
                },
                UrlData = @url.Action("LoadChartCategoriaVlr", "DashboardContaPagar"),
                Class = "col s12",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });
            cfg.Content.Add(new ChartUI
            {
                Id = "chartCategoriaQtd",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Quantidade - Total",
                        fontSize = 15,
                        fontFamily = "Roboto",
                        fontColor = "#555"
                    },
                    tooltips = new
                    {
                        mode = "label",
                        bodySpacing = 10,
                        cornerRadius = 0,
                        titleMarginBottom = 15
                    },
                    legend = new { position = "bottom" },
                    global = new
                    {
                        responsive = false,
                        maintainAspectRatio = false
                    },
                    scales = new
                    {
                        xAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            },
                        yAxes = new object[] {
                                new
                                {
                                    stacked = true
                                }
                            }
                    }
                },
                UrlData = @url.Action("LoadChartCategoriaQtd", "DashboardContaPagar"),
                Class = "col s12",
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

        // Load Status
        public JsonResult LoadChartStatusVlr(DateTime dataInicial)
        {
            var response = GetProjecaoStatus(dataInicial);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[] {
                    new {
                            label = "Valor",
                            fill = false,
                            backgroundColor = new string[] { "rgb(75, 192, 192)", "rgb(255, 99, 132)"},
                            borderColor = new string[] { "rgb(75, 192, 192)", "rgb(255, 99, 132)"},
                            data = response.Select(x => Math.Round(x.Total, 2)).ToArray(),
                    }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadChartStatusQtd(DateTime dataInicial)
        {
            var response = GetProjecaoStatus(dataInicial);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[] {
                    new {
                            label = "Quantidade",
                            fill = true,
                            backgroundColor = new string[] { "rgb(75, 192, 192)", "rgb(255, 99, 132)"},
                            borderColor = new string[] { "rgb(75, 192, 192)", "rgb(255, 99, 132)"},
                            data = response.Select(x => (x.Quantidade)).ToArray()
                        }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }
        // Load Pagamento
        public JsonResult LoadChartPagamentoVlr(DateTime dataInicial)
        {
            var response = GetProjecaoPagamento(dataInicial);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[] {
                    new {
                            label = "Valor",
                            fill = false,
                            backgroundColor = "rgb(75, 192, 192)",
                            borderColor = "rgb(75, 192, 192)",
                            data = response.Select(x => Math.Round(x.Total, 2)).ToArray()
                    }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadChartPagamentoQtd(DateTime dataInicial)
        {
            var response = GetProjecaoPagamento(dataInicial);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[] {
                    //new {
                    //        label = "Valor",
                    //        fill = false,
                    //        backgroundColor = "rgb(75, 192, 192)",
                    //        borderColor = "rgb(75, 192, 192)",
                    //        data = response.Select(x => Math.Round(x.Total, 2)).ToArray()
                    //},
                    new {

                            label = "Quantidade",
                            fill = false,
                            backgroundColor = "rgb(255, 99, 132)",
                            borderColor = "rgb(255, 99, 132)",
                            data = response.Select(x => (x.Quantidade)).ToArray(),
                        }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }
        // Load Categoria
        public JsonResult LoadChartCategoriaVlr(DateTime dataInicial)
        {
            var response = GetProjecaoCategoria(dataInicial);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[] {
                    new {
                            label = "Valor",
                            fill = false,
                            backgroundColor = "rgb(75, 192, 192)",
                            borderColor = "rgb(75, 192, 192)",
                            data = response.Select(x => Math.Round(x.Total, 2)).ToArray()
                    }
                    //,
                    //new {
                    //        label = "Quantidade",
                    //        fill = false,
                    //        backgroundColor = "rgb(255, 99, 132)",
                    //        borderColor = "rgb(255, 99, 132)",
                    //        data = response.Select(x => (x.Quantidade)).ToArray(),
                    //    }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadChartCategoriaQtd(DateTime dataInicial)
        {
            var response = GetProjecaoCategoria(dataInicial);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[] {
                    //new {
                    //        label = "Valor",
                    //        fill = false,
                    //        backgroundColor = "rgb(75, 192, 192)",
                    //        borderColor = "rgb(75, 192, 192)",
                    //        data = response.Select(x => Math.Round(x.Total, 2)).ToArray()
                    //},
                    new {
                            label = "Quantidade",
                            fill = false,
                            backgroundColor = "rgb(255, 99, 132)",
                            borderColor = "rgb(255, 99, 132)",
                            data = response.Select(x => (x.Quantidade)).ToArray(),
                        }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }

        private List<DashboardContaPagarCategoriaVM> GetProjecaoCategoria(DateTime dataInicial)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", "ContaPagar" }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaPagarCategoriaVM>>("dashboardcategoria", queryString);
            if (response == null)
                return new List<DashboardContaPagarCategoriaVM>();

            return response;
        }
        private List<DashboardContaPagarStatusVM> GetProjecaoStatus(DateTime dataInicial)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", "ContaPagar" }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaPagarStatusVM>>("dashboardstatus", queryString);
            if (response == null)
                return new List<DashboardContaPagarStatusVM>();

            return response;

        }
        private List<DashboardContaPagarFormaPagamentoVM> GetProjecaoPagamento(DateTime dataInicial)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", "ContaPagar" }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaPagarFormaPagamentoVM>>("dashboardformapagamento", queryString);
            if (response == null)
                return new List<DashboardContaPagarFormaPagamentoVM>();

            return response;
        }

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
    }
}
