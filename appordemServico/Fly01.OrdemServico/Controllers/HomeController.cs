using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Widgets;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string buttonLabel, string buttonOnClick)
        {
            var target = new List<HtmlUIButton>();
            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "filterGrid1", Label = buttonLabel, OnClickFn = buttonOnClick });     
            }

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                target.Add(new HtmlUIButton { Id = "filterGrid2", Label = "Atualizar", OnClickFn = "fnAtualizar" });
            }
                return target;
        }

        protected override ContentUI HomeJson()
            => HomeJ();

        public ContentResult ListHomeJson(string gridLoad)
        {
            return Content(JsonConvert.SerializeObject(HomeJ(gridLoad), JsonSerializerSetting.Front), "application/json");
        }
        protected ContentUI HomeJ(string gridLoad = "GridLoad")
        {
            if (!UserCanPerformOperation(ResourceHashConst.OrdemServicoVisaoGeral))
                return new ContentUI { SidebarUrl = @Url.Action("Sidebar") };

            var buttonLabel = "Mostrar O.S. por período";
            var buttonOnClick = "fnRemoveFilter";


            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Mostrar O.S. do mês atual";
                buttonOnClick = "fnAddFilter";
            }
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                SidebarUrl = Url.Action("Sidebar"),
                Header = new HtmlUIHeader()
                {
                    Title = "Visão Geral",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                Functions = new List<string> { "__format", "fnGetSaldos" }
            };
            var dataInicialFiltroDefault = DateTime.Now.Date;

            var dataFinal = DateTime.Now.AddMonths(1);
            var dataFinalFiltroDefault = new DateTime(dataFinal.Year, dataFinal.Month, DateTime.DaysInMonth(dataFinal.Year, dataFinal.Month));


            if (Request.QueryString["action"] != "GridLoadNoFilter")
            {
                cfg.Content.Add(new FormUI
                {
                    Id = "fly01frm",
                    ReadyFn = "fnFormReady",
                    UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                    Class = "col s12",
                    Elements = new List<BaseUI>
                {
                    new PeriodPickerUI()
                    {
                        Label = "Selecione o período",
                        Id = "mesPicker",
                        Name = "mesPicker",
                        Class = "col s12 m6 offset-m3 l4 offset-l4",
                        DomEvents = new List<DomEventUI>()
                        {
                            new DomEventUI()
                            {
                                DomEvent = "change",
                                Function = "fnUpdateDataFinal"
                            }
                        }
                    },
                    new InputHiddenUI()
                    {
                        Id = "dataFinal",
                        Name = "dataFinal",
                    },
                    new InputHiddenUI()
                    {
                        Id = "dataInicial",
                        Name = "dataInicial"

                    }
                }
                });
            }
            else
            {
                cfg.Content.Add(new FormUI
                {
                    Id = "fly01frm2",
                    ReadyFn = "fnFormReadyPeriodo",
                    UrlFunctions = Url.Action("Functions") + "?fns=",
                    Class = "col s12",
                    Elements = new List<BaseUI>
                {
                    new InputDateUI {
                        Id =  "dataInicial",
                        Class = "col s6 m3 l4",
                        Label = "Data Inicial"
                    },
                    new InputDateUI {
                        Id =  "dataFinal",
                        Class = "col s6 m3 l4",
                        Label = "Data Final"
                    },
                    new ButtonGroupUI()
                    {
                        Id = "fly01btngrp",
                        Class = "col s12 m6 l4",
                        Label = "Selecione o período",
                        OnClickFn = "fnAtualizarPeriodo",
                        Options = new List<ButtonGroupOptionUI>
                        {
                            new ButtonGroupOptionUI {Id = "btnDia", Value = "dia", Label = "Dia", Class = "col s4"},
                            new ButtonGroupOptionUI {Id = "btnSemana", Value = "semana", Label = "Semana", Class = "col s4"},
                            new ButtonGroupOptionUI {Id = "btnMes", Value = "mes", Label = "Mês", Class = "col s4"}
                        }
                    }
                }
                });

            }

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = GetColor(StatusOrdemServico.EmAberto),
                Id = "fly01cardAB",
                Title = GetDescription(StatusOrdemServico.EmAberto),
                Placeholder = "0/0",
                Action = new LinkUI
                {
                    Label = "",
                    OnClick = ""
                }
            });
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = GetColor(StatusOrdemServico.EmAndamento),
                Id = "fly01cardAN",
                Title = GetDescription(StatusOrdemServico.EmAndamento),
                Placeholder = "0/0",
                Action = new LinkUI
                {
                    Label = "",
                    OnClick = ""
                }
            });
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = GetColor(StatusOrdemServico.Concluido),
                Id = "fly01cardCO",
                Title = GetDescription(StatusOrdemServico.Concluido),
                Placeholder = "0/0",
                Action = new LinkUI
                {
                    Label = "",
                    OnClick = ""
                }
            });
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = GetColor(StatusOrdemServico.Cancelado),
                Id = "fly01cardCA",
                Title = GetDescription(StatusOrdemServico.Cancelado),
                Placeholder = "0/0",
                Action = new LinkUI
                {
                    Label = "",
                    OnClick = ""
                }
            });

            cfg.Content.Add(new ChartUI
            {
                Id = "fly01chart",
                Options = new
                {
                    title = new
                    {
                        display = true,
                        text = "Ordens de Serviço por dia",
                        fontSize = 12,
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
                        xAxes = new object[] { new { stacked = true } },
                        yAxes = new object[] { new { stacked = true, ticks = new { stepSize = 1 } } }
                    }
                },
                UrlData = @Url.Action("LoadChart", "Dashboard"),
                Class = "col s12",
                Parameters = new List<ChartUIParameter>
                {
                    new ChartUIParameter { Id = "dataFinal" }
                }
            });

            cfg.Content.Add(new DivUI
            {
                Elements = new List<BaseUI>
                {
                    new LabelSetUI { Id = "t2", Class = "col s6", Label = "Os 10 serviços mais prestados" },
                    new LabelSetUI { Id = "t1", Class = "col s6", Label = "Os 10 produtos mais vendidos" }
                }
            });
            cfg.Content.Add(new DataTableUI
            {
                Class = "col s6",
                Id = "dtGridTopServicos",
                UrlGridLoad = Url.Action("DashboardTopServicos", "Dashboard"),
                Parameters = new List<DataTableUIParameter>
                    {
                        new DataTableUIParameter { Id = "dataFinal" }
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
                Id = "dtGridTopProdutos",
                UrlGridLoad = Url.Action("DashboardTopProdutos", "Dashboard"),
                Parameters = new List<DataTableUIParameter>
                    {
                        new DataTableUIParameter { Id = "dataFinal" }
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


            return cfg;
        }

        private static string GetColor(StatusOrdemServico statusOrdem) => EnumHelper.GetCSS(typeof(StatusOrdemServico), statusOrdem.ToString());
        private static string GetDescription(StatusOrdemServico statusOrdem)
        {
            var result = EnumHelper.GetDescription(typeof(StatusOrdemServico), statusOrdem.ToString());
            return string.IsNullOrEmpty(result) ? "" : $"{char.ToUpper(result[0])}{result.Substring(1).ToLower()}";
        }

        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Ordem de Serviço", Parent = "header", PlatformsUrl = @Url.Action("Platforms", "Account") };

            config.Notification = new SidebarUINotification()
            {
                Channel = AppDefaults.AppId + "_" + SessionManager.Current.UserData.PlatformUrl,
                JWT = @Url.Action("NotificationJwt"),
                SocketServer = AppDefaults.UrlNotificationSocket
            };

            #region MenuItems
            var menuItems = new List<SidebarUIMenu>()
            {
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.OrdemServico,
                    Label = "Ordem de Serviço",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoVisaoGeral, Label = "Visão Geral", OnClick = @Url.Action("List", "Home")},
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoOrdemServico, Label = "Ordem de Serviço", OnClick = @Url.Action("List", "OrdemServico")},
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoAgenda, Label = "Agenda", OnClick = @Url.Action("List", "Agenda")},
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.OrdemServicoCadastros,
                    Label = "Cadastro",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoCadastrosProdutos, Label = "Produtos", OnClick = @Url.Action("List", "Produto")},
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoCadastrosServicos, Label = "Serviços", OnClick = @Url.Action("List", "Servico")},
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoCadastrosClientes, Label = "Clientes", OnClick = @Url.Action("List", "Cliente")},
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoCadastrosResponsaveis, Label = "Responsáveis", OnClick = @Url.Action("List", "Responsavel")},
                        new LinkUI() { Class = ResourceHashConst.FaturamentoCadastrosKit, Label = "Kit Produtos/Serviços", OnClick = @Url.Action("List", "Kit") }
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.OrdemServicoConfiguracoes,
                    Label = "Configurações",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoConfiguracoesParametros, Label = "Parâmetros", OnClick = @Url.Action("List", "ParametroOrdemServico")},
                        //Personalizar Sistema não vai ter hash especifico de permissão, segundo Fraga
                        //new LinkUI() { Class = ResourceHashConst.OrdemServicoConfiguracoes, Label = "Personalizar Sistema", OnClick = @Url.Action("Form", "ConfiguracaoPersonalizacao") }
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.OrdemServicoAjuda,
                    Label = "Ajuda",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoAjudaAssistenciaRemota, Label =  "Assistência Remota", OnClick = @Url.Action("Form", "AssistenciaRemota") },
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoAjuda,Label = "Manual do Usuário", Link = "https://centraldeatendimento.totvs.com/hc/pt-br/categories/360000364572" }
                    }
                },
                new SidebarUIMenu() { Class = ResourceHashConst.OrdemServicoAvalieAplicativo, Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") }
            };

            config.MenuItems.AddRange(ProcessMenuRoles(menuItems));
            #endregion

            #region User Menu Items
            if (!string.IsNullOrEmpty(SessionManager.Current.UserData.TokenData.CodigoMaxime))
                config.UserMenuItems.Add(new LinkUI() { Label = "Minha Conta", OnClick = @Url.Action("List", "MinhaConta") });
            config.UserMenuItems.Add(new LinkUI() { Label = "Sair", Link = @Url.Action("Logoff", "Account") });
            #endregion

            #region Lista de aplicativos do usuário
            config.MenuApps.AddRange(AppsList());
            #endregion

            config.Name = SessionManager.Current.UserData.TokenData.UserName;
            config.Email = SessionManager.Current.UserData.PlatformUser;

            config.Widgets = new WidgetsUI
            {
                Conpass = new ConpassUI(),
                Droz = new DrozUI(),
                Zendesk = new ZendeskUI()
                {
                    AppName = "Bemacash Gestão",
                    AppTag = "chat_fly01_gestao",
                }
            };
            if (Request.Url.ToString().Contains("bemacash.com.br"))
                config.Widgets.Insights = new InsightsUI { Key = ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"] };

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }

}