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
                       Class= "col s12 m6 l4 offset-l2",
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
                        Id = "tpOrdemCompra",
                        Name = "tpOrdemCompra"
                    },
                    new ButtonGroupUI()
                    {
                        Id = "fly01btngrp",
                        Class = "col s12 m6 l4",
                        Label = "Tipo",
                        OnClickFn = "fnChangeTipoOrdemCompra",
                        Options = new List<ButtonGroupOptionUI>
                        {
                            new ButtonGroupOptionUI { Id = "btnOrcamento", Value = "Orcamento", Label = "Orçamento", Class = "col s6" },
                            new ButtonGroupOptionUI { Id = "btnPedido", Value = "Pedido", Label = "Pedido", Class = "col s6" }
                        }
                    }
                }
            });
            // CHART Status Valor
            cfg.Content.Add(new ChartUI
            {
                Id = "chartStatus",
                DrawType = ChartUIType.Doughnut,
                Options = ChartOptions("Status"),
                UrlData = Url.Action("Status", "Dashboard"),
                Class = "col s12 m4",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" },
                        new ChartUIParameter { Id = "tpOrdemCompra" }
                    }
            });

            // CHART Forma de Pagamento
            cfg.Content.Add(new ChartUI
            {
                Id = "chartCategoria",
                DrawType = ChartUIType.Doughnut,
                Options = ChartOptions("Categoria"),
                UrlData = Url.Action("Categoria", "Dashboard"),
                Class = "col s12 m4",
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
                UrlData = Url.Action("FormaPagamento", "Dashboard"),
                Class = "col s12 m4",
                Parameters = new List<ChartUIParameter>
                    {
                        new ChartUIParameter { Id = "dataInicial" },
                        new ChartUIParameter { Id = "tpOrdemCompra" }
                    }
            });
            cfg.Content.Add(new DivUI
            {
                Elements = new List<BaseUI>
                {
                    new LabelSetUI { Id = "t1", Class = "col s6", Label = "Os 10 produtos mais comprados" },
                    new LabelSetUI { Id = "t2", Class = "col s6", Label = "Os 10 maiores fornecedores" }
                }
            });
            cfg.Content.Add(new DataTableUI
            {
                Class = "col s6",
                Id = "mProd",
                UrlGridLoad = Url.Action("MaisComprados", "Dashboard"),
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
                        DataField = "quantidade",
                        DisplayName = "Quantidade",
                        Class = "dt-right",
                        Priority = 4,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "valorTotal",
                        DisplayName = "Total",
                        Priority = 5,
                        Type = "currency",
                        Orderable = false,
                        Searchable = false
                    },

                }
            });
            cfg.Content.Add(new DataTableUI
            {
                Class = "col s6",
                Id = "mforn",
                UrlGridLoad = Url.Action("MaioresFornecedores", "Dashboard"),
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
                        DataField = "nome",
                        DisplayName = "Nome",
                        Priority = 2,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "valor",
                        DisplayName = "Valor",
                        Priority = 3,
                        Type = "currency",
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
                        new LinkUI() { Class = ResourceHashConst.ComprasComprasDashboard, Label = "Dashboard", OnClick = @Url.Action("List", "Home")},
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
    }
}
