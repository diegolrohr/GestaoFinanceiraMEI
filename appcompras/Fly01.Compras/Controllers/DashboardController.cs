using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
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

namespace Fly01.Compras.Controllers
{
    public class DashboardController : BaseController<DashboardVM>
    {
        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public override Func<DashboardVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
            => Content(JsonConvert.SerializeObject(DashboardJson(Url, Request.Url.Scheme), JsonSerializerSetting.Front), "application/json");

        public static ContentUI DashboardJson(UrlHelper url, string scheme, bool withSidebarUrl = false)
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = url.Action("Index", "Dashboard") },
                Header = new HtmlUIHeader
                {
                    Title = "Dashboard"
                },
                UrlFunctions = url.Action("Functions", "Dashboard") + "?fns="
            };

            if (withSidebarUrl)
                cfg.SidebarUrl = url.Action("Sidebar", "Dashboard", null, scheme);

            cfg.Content.Add(new FormUI
            {
                Id = "filterForm",
                ReadyFn = "fnFormReady",
                UrlFunctions = url.Action("Functions", "Dashboard") + "?fns=",
                Functions = new List<string> { "__format" },
                Class = "col s12",
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
                        Class = "col s12 m6 l6",
                        OnClickFn = "fnAtualizarChart",
                        Options = new List<ButtonGroupOptionUI>
                        {
                            new ButtonGroupOptionUI {Id = "btnStatus", Value = "1", Label = "Status", Class="col s6"},
                            new ButtonGroupOptionUI {Id = "btnFormaPagamento", Value = "2", Label = "Forma de Pagamento", Class="col s6"}
                        }
                    },
                    new SelectUI
                    {
                        Id = "tpOrdemCompra",
                        Class = "col s12 m6 l6",
                        Disabled = false,
                        Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoOrdemCompra)).ToList()),
                        DomEvents = new List<DomEventUI>() { new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoOrdemCompra" } }
                    }
                }
            });
            // CHART Status Valor
            cfg.Content.Add(new ChartUI
            {
                Id = "chartStatusValor",
                Class = "col s12 m12 l6",
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
                UrlData = @url.Action("LoadChartStatusValor", "Dashboard"),
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" },
                        new ChartUIParameter { Id = "tpOrdemCompra" }
                    }
            });
            // CHART Status Quantidade
            cfg.Content.Add(new ChartUI
            {
                Id = "chartStatusQtd",
                Class = "col s12 m12 l6",
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
                UrlData = @url.Action("LoadChartStatusQuantidade", "Dashboard"),
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" },
                        new ChartUIParameter { Id = "tpOrdemCompra" }
                    }
            });
            // CHART Forma de Pagamento
            cfg.Content.Add(new ChartUI
            {
                Id = "chartPagamentoValor",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Forma de Pagamento - Valor",
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
                UrlData = @url.Action("LoadChartFormaPagamentoValor", "Dashboard"),
                Class = "col s12 m6 l6",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" },
                        new ChartUIParameter { Id = "tpOrdemCompra" }
                    }
            });
            // CHART Forma de Pagamento
            cfg.Content.Add(new ChartUI
            {
                Id = "chartPagamentoQtd",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Forma de Pagamento - Quantidade",
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
                UrlData = @url.Action("LoadChartFormaPagamentoQtd", "Dashboard"),
                Class = "col s12 m6 l6",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" },
                        new ChartUIParameter { Id = "tpOrdemCompra" }
                    }
            });
            cfg.Content.Add(new DivUI
            {
                Elements = new List<BaseUI>{
                    new LabelSetUI { Id = "titleLabel", Class = "col s12", Label = "TOP 10 - PRODUTOS MAIS COMPRADOS" }
                }
            });
            cfg.Content.Add(new DataTableUI
            {
                UrlGridLoad = url.Action("DashboardGridLoad", "Dashboard"),
                Parameters = new List<DataTableUIParameter>
                    {
                        new DataTableUIParameter { Id = "dataInicial" }
                    },
                Options = new DataTableUIConfig()
                {
                    PageLength = 10,
                    WithoutRowMenu = true
                },
                Columns = new List<DataTableUIColumn>{
                    new DataTableUIColumn
                    {
                        DataField = "descricao",
                        DisplayName = "Descrição",
                        Priority = 2,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "unidadeMedida",
                        DisplayName = "Unidade de Medida",
                        Priority = 1,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "valor",
                        DisplayName = "Valor",
                        Priority = 3,
                        Type = "valor",
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "quantidade",
                        DisplayName = "Quantidade",
                        Priority = 4,
                        Type = "valor",
                        Orderable = false,
                        Searchable = false
                    }
                }
            });
            return cfg;
        }

        [HttpGet]
        public JsonResult DashboardGridLoad(DateTime? dataInicial = null)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>
                    {
                        { "filtro", dataInicial.HasValue? dataInicial.Value.ToString("yyyy-MM-dd"):DateTime.Now.ToString("yyyy-MM-dd") }
                    };

                var response = RestHelper.ExecuteGetRequest<List<DashboardGridVM>>("dashboardprodutosmaiscomprados", queryString);

                return Json(new
                {
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count,
                    data = response.Select(x => new
                    {
                        descricao = x.Descricao,
                        unidadeMedida = x.UnidadeMedida,
                        valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                        quantidade = x.Quantidade
                    })
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult LoadChartStatusQuantidade(DateTime dataInicial, String tpOrdemCompra)
        {
            var response = GetProjecaoStatus(dataInicial, tpOrdemCompra);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Status).ToArray(),
                datasets = new object[] {
                    //new {
                    //        label = "Valor",
                    //        fill = false,
                    //        backgroundColor = "rgb(75, 192, 192)",
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

        public JsonResult LoadChartStatusValor(DateTime dataInicial, String tpOrdemCompra)
        {
            var response = GetProjecaoStatus(dataInicial, tpOrdemCompra);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Status).ToArray(),
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
                    //        data = response.Select(x => (x.Quantidade)).ToArray()
                    //    }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadChartFormaPagamentoValor(DateTime dataInicial, String tpOrdemCompra)
        {
            var response = GetProjecaoFormaPagamento(dataInicial, tpOrdemCompra);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.TipoFormaPagamento).ToArray(),
                datasets = new object[] {
                    new {
                            label = "Valor",
                            fill = false,
                            backgroundColor = "rgb(75, 192, 192)",
                            data = response.Select(x => Math.Round(x.Total, 2)).ToArray()
                    }
                    //,
                    //new {
                    //        label = "Quantidade",
                    //        fill = false,
                    //        backgroundColor = "#f58542",
                    //        data = response.Select(x => (x.Quantidade)).ToArray(),
                    //    }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadChartFormaPagamentoQtd(DateTime dataInicial, String tpOrdemCompra)
        {
            var response = GetProjecaoFormaPagamento(dataInicial, tpOrdemCompra);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.TipoFormaPagamento).ToArray(),
                datasets = new object[] {
                    //new {
                    //        label = "Valor",
                    //        fill = false,
                    //        backgroundColor = "rgb(75, 192, 192)",
                    //        data = response.Select(x => Math.Round(x.Total, 2)).ToArray()
                    //}
                    ////,
                    new {
                            label = "Quantidade",
                            fill = false,
                            backgroundColor =  "rgb(255, 99, 132)",
                            data = response.Select(x => (x.Quantidade)).ToArray(),
                        }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }

        private List<DashboardStatusVM> GetProjecaoStatus(DateTime dataInicial, String tpOrdemCompra)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", tpOrdemCompra}
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardStatusVM>>("dashboardstatus", queryString);
            if (response == null)
                return new List<DashboardStatusVM>();

            return response;

        }

        private List<DashboardFormaPagamentoVM> GetProjecaoFormaPagamento(DateTime dataInicial, String tpOrdemCompra)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", tpOrdemCompra}
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardFormaPagamentoVM>>("dashboardformaspagamento", queryString);
            if (response == null)
                return new List<DashboardFormaPagamentoVM>();

            return response;
        }
    }
}
