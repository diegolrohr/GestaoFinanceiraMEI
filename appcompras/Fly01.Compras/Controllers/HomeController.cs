using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Widgets;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        protected override ContentUI HomeJson(bool withSidebarUrl = false)
        {
            if (!UserCanPerformOperation(ResourceHashConst.ComprasComprasDashboard))
                return new ContentUI();

            //return DashboardJson(Url, Request.Url.Scheme, withSidebarUrl);
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Dashboard"
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            if (withSidebarUrl)
                cfg.SidebarUrl = Url.Action("Sidebar");

            cfg.Content.Add(new FormUI
            {
                Id = "filterForm",
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Class = "col s12",
                Elements = new List<BaseUI>()
                {
                    new PeriodPickerUI()
                    {
                       Label= "Selecione o período",
                       Id= "mesPicker",
                       Name= "mesPicker",
                       Class= "col s12 m6",
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
                    new SelectUI
                    {
                        Id = "tpOrdemCompra",
                        Class = "col s12 m6",
                        Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoOrdemCompra)).ToList()),
                        DomEvents = new List<DomEventUI>() { new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoOrdemCompra" } }
                    }
                }
            });
            // CHART Status Valor
            cfg.Content.Add(new ChartUI
            {
                Id = "chartStatus",
                DrawType = ChartUIType.Doughnut,
                Options = ChartOptions("Status"),
                UrlData = Url.Action("LoadChartStatus"),
                Class = "col s12 m6",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" },
                        new ChartUIParameter { Id = "tpOrdemCompra" }
                    }
            });

            // CHART Forma de Pagamento
            cfg.Content.Add(new ChartUI
            {
                Id = "chartPagamento",
                DrawType = ChartUIType.Doughnut,
                Options = ChartOptions("Forma de Pagamento"),
                UrlData = Url.Action("LoadChartFormaPagamento"),
                Class = "col s12 m6",
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
                UrlGridLoad = Url.Action("DashboardGridLoad"),
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

        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Compras", Parent = "header" };

            #region MenuItems
            var menuItems = new List<SidebarUIMenu>()
            {
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.ComprasCompras,
                    Label = "Compras",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.ComprasComprasDashboard, Label = "Dashboard", OnClick = @Url.Action("List", "Dashboard")},
                        new LinkUI() { Class = ResourceHashConst.ComprasComprasOrcamentoPedido, Label = "Orçamentos/Pedidos", OnClick = @Url.Action("List", "OrdemCompra")},
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.ComprasCadastros,
                    Label = "Cadastros",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosProdutos, Label = "Produtos", OnClick = @Url.Action("List", "Produto")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosFornecedores, Label = "Fornecedores", OnClick = @Url.Action("List", "Fornecedor")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosTransportadora, Label = "Transportadoras", OnClick = @Url.Action("List", "Transportadora")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosCondicoesParcelamento, Label = "Condições de Parcelamento",OnClick = @Url.Action("List", "CondicaoParcelamento")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosFormaPagamento, Label = "Forma de Pagamento", OnClick = @Url.Action("List", "FormaPagamento")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosGrupoTributario, Label = "Grupo Tributário", OnClick = @Url.Action("List", "GrupoTributario")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosGrupoProdutos, Label = "Grupo de Produtos", OnClick = @Url.Action("List", "GrupoProduto")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosCategoria, Label = "Categoria", OnClick = @Url.Action("List", "Categoria")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosSubstituicaoTributaria, Label = "Substituição Tributária", OnClick = @Url.Action("List", "SubstituicaoTributaria")}
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.ComprasAjuda,
                    Label = "Ajuda",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.ComprasAjudaAssistenciaRemota, Label =  "Assistência Remota", Link = "https://secure.logmeinrescue.com/customer/code.aspx"}
                    }
                },
                new SidebarUIMenu() { Class = ResourceHashConst.ComprasAvalieAplicativo, Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") }
            };

            config.MenuItems.AddRange(ProcessMenuRoles(menuItems));
            #endregion

            #region User Menu Items
            config.UserMenuItems.Add(new LinkUI() { Label = "Sair", Link = @Url.Action("Logoff", "Account") });
            #endregion

            #region Lista de aplicativos do usuário
            config.MenuApps.AddRange(AppsList());
            #endregion

            config.Name = SessionManager.Current.UserData.TokenData.Username;
            config.Email = SessionManager.Current.UserData.PlatformUser;

            config.Widgets = new WidgetsUI
            {
                Conpass = new ConpassUI(),
                Droz = new DrozUI(),
                Zendesk = new ZendeskUI()
                {
                    AppName = "Fly01 Gestão",
                    AppTag = "chat_fly01_gestao",
                }
            };
            if (Request.Url.ToString().Contains("fly01.com.br"))
                config.Widgets.Insights = new InsightsUI { Key = ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"] };

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

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

        public JsonResult LoadChartStatus(DateTime dataInicial, String tpOrdemCompra)
        {
            var response = GetProjecaoStatus(dataInicial, tpOrdemCompra);

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
        public JsonResult LoadChartFormaPagamento(DateTime dataInicial, String tpOrdemCompra)
        {
            var response = GetProjecaoFormaPagamento(dataInicial, tpOrdemCompra);

            var dataChartToView = new
            {
                success = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[] {
                    new {
                            label = "Valor",
                            fill = false,
                            backgroundColor = "rgb(75, 192, 192)",
                            data = response.Select(x => Math.Round(x.Total, 2)).ToArray()
                    }
                }
            };

            return Json(dataChartToView, JsonRequestBehavior.AllowGet);
        }

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

        private List<DashboardComprasVM> GetProjecaoStatus(DateTime dataInicial, String tpOrdemCompra) => GetProjecao(dataInicial, tpOrdemCompra, "dashboardstatus");
        private List<DashboardComprasVM> GetProjecaoFormaPagamento(DateTime dataInicial, String tpOrdemCompra) => GetProjecao(dataInicial, tpOrdemCompra, "dashboardformaspagamento");
        protected List<DashboardComprasVM> GetProjecao(DateTime dataInicial, String tpOrdemCompra, string resource)
        {
            const int topCount = 4;
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") },
                { "tipo", tpOrdemCompra }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardComprasVM>>(resource, queryString);
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
    }
}