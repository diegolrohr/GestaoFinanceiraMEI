﻿using Fly01.Core.Presentation;
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
    public class DashboardContaReceberController : BaseController<DashboardContaReceberVM>
    {
        public override ContentResult Form() { throw new NotImplementedException(); }

        public override Func<DashboardContaReceberVM, object> GetDisplayData() { throw new NotImplementedException(); }

        public override ContentResult List()
        {
            return Content(JsonConvert.SerializeObject(DashboardContaReceberJson(Url, Request.Url.Scheme), JsonSerializerSetting.Front), "application/json");
        }

        protected internal static ContentUI DashboardContaReceberJson(UrlHelper url, string scheme, bool withSidebarUrl = false)
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = url.Action("Index", "DashboardContaReceber") },
                Header = new HtmlUIHeader
                {
                    Title = "Dashboard - Contas a Receber"
                },
                UrlFunctions = url.Action("Functions", "DashboardContaReceber") + "?fns="
            };
            if (withSidebarUrl)
                cfg.SidebarUrl = url.Action("Sidebar", "DashboardContaReceber", null, scheme);


            var cfgForm = new FormUI
            {
                ReadyFn = "fnUpdateDataFinal",
                UrlFunctions = url.Action("Functions", "DashboardContaReceber") + "?fns=",
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
                }
            };

            cfg.Content.Add(cfgForm);

            //OPÇÕES
            cfg.Content.Add(new FormUI
            {
                Id = "btnOpcoes",
                ReadyFn = "fnFormReady",
                Functions = new List<string> { "__format" },
                UrlFunctions = url.Action("Functions", "DashboardContaReceber", null) + "?fns=",
                Class = "col s12 m8 offset-m2",
                Elements = new List<BaseUI>
                {
                    new ButtonGroupUI
                    {
                        Id = "fly01btngrp",
                        Class = "col s12",
                        OnClickFn = "fnAtualizarChart",
                        Options = new List<ButtonGroupOptionUI>
                        {
                            new ButtonGroupOptionUI {Id = "btnStatus", Value = "1", Label = "Status", Class="col s3"},
                            new ButtonGroupOptionUI {Id = "btnFormaPagamento", Value = "2", Label = "Forma de Pagamento", Class="col s6"},
                            new ButtonGroupOptionUI {Id = "btnCategoria", Value = "3", Label = "Categoria", Class="col s3"}
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
                UrlData = @url.Action("LoadChartStatusVlr", "DashboardContaReceber"),
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
                UrlData = @url.Action("LoadChartStatusQtd", "DashboardContaReceber"),
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
                UrlData = @url.Action("LoadChartPagamentoVlr", "DashboardContaReceber"),
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
                UrlData = @url.Action("LoadChartPagamentoQtd", "DashboardContaReceber"),
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
                UrlData = @url.Action("LoadChartCategoriaVlr", "DashboardContaReceber"),
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
                UrlData = @url.Action("LoadChartCategoriaQtd", "DashboardContaReceber"),
                Class = "col s12",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });

            // CHART Dia/Dia
            cfg.Content.Add(new ChartUI
            {
                Id = "chartSaldoDiaDia",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Recebido - Dia/Dia",
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
                UrlData = @url.Action("LoadChartSaldoDia", "DashboardContaReceber"),
                Class = "col s12",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" }
                    }
            });
            return cfg;
        }

        [HttpGet]
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
                    //,
                    //new {
                    //        label = "Quantidade",
                    //        fill = false,
                    //        backgroundColor = "rgb(255, 99, 132)",
                    //        borderColor = "rgb(255, 99, 132)",
                    //        data = response.Select(x => (x.Quantidade)).ToArray()
                    //    }
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
                    //new {
                    //        label = "Valor",
                    //        fill = false,
                    //        backgroundColor = "rgb(75, 192, 192)",
                    //        borderColor = "rgb(75, 192, 192)",
                    //        data = response.Select(x => Math.Round(x.Total, 2)).ToArray(),
                    //},
                    new {
                            label = "Quantidade",
                            fill = false,
                            backgroundColor = new string[] { "rgb(75, 192, 192)", "rgb(255, 99, 132)"},
                            borderColor = new string[] { "rgb(75, 192, 192)", "rgb(255, 99, 132)"},
                            data = response.Select(x => (x.Quantidade)).ToArray()
                        }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }

        private List<DashboardContaReceberStatusVM> GetProjecaoStatus(DateTime dataInicial)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", "ContaReceber" }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaReceberStatusVM>>("dashboardstatus", queryString);
            if (response == null)
                return new List<DashboardContaReceberStatusVM>();

            return response;

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

        private List<DashboardContaReceberFormaPagamentoVM> GetProjecaoPagamento(DateTime dataInicial)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", "ContaReceber" }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaReceberFormaPagamentoVM>>("dashboardformapagamento", queryString);
            if (response == null)
                return new List<DashboardContaReceberFormaPagamentoVM>();

            return response;
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

        private List<DashboardContaReceberCategoriaVM> GetProjecaoCategoria(DateTime dataInicial)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", "ContaReceber" }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaReceberCategoriaVM>>("dashboardcategoria", queryString);
            if (response == null)
                return new List<DashboardContaReceberCategoriaVM>();

            return response;
        }

        // Load SaldoDia
        public JsonResult LoadChartSaldoDia(DateTime dataInicial)
        {
            var response = GetProjecaoSaldoDia(dataInicial);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Dia).ToArray(),
                datasets = new object[] {
                    new {
                            type = "line",
                            label = "Quantidade",
                            fill = false,
                            backgroundColor = "rgb(99, 99, 99)",
                            borderColor = "rgb(99, 99, 99)",
                            data = response.Select(x => (x.Total)).ToArray(),
                        }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }

        private List<DashboardContaReceberSaldoDiaVM> GetProjecaoSaldoDia(DateTime dataInicial)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaReceberSaldoDiaVM>>("dashboardsaldodia", queryString);
            if (response == null)
                return new List<DashboardContaReceberSaldoDiaVM>();

            return response;
        }
    }
}