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
        protected override ContentUI HomeJson()
        {
            if (!UserCanPerformOperation(ResourceHashConst.OrdemServicoVisaoGeral))
                return new ContentUI { SidebarUrl = @Url.Action("Sidebar") };

            //ManagerEmpresaVM response = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
            //var responseCidade = response.Cidade != null ? response.Cidade.Nome : string.Empty;

            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                SidebarUrl = Url.Action("Sidebar"),
                Header = new HtmlUIHeader()
                {
                    Title = "Visão Geral",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                Functions = new List<string> { "__format", "fnGetSaldos" }
            };
            var dataInicialFiltroDefault = DateTime.Now.Date;

            var dataFinal = DateTime.Now.AddMonths(1);
            var dataFinalFiltroDefault = new DateTime(dataFinal.Year, dataFinal.Month, DateTime.DaysInMonth(dataFinal.Year, dataFinal.Month));

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
                        Name = "dataFinal"
                    },
                    new InputHiddenUI()
                    {
                        Id = "dataInicial",
                        Name = "dataInicial"

                    }
                }
            });

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
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Ordem de Serviço", Parent = "header" };

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
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.OrdemServicoCadastros,
                    Label = "Cadastro",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoCadastroProdutos, Label = "Produtos", OnClick = @Url.Action("List", "Produto")},
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoCadastroServicos, Label = "Serviços", OnClick = @Url.Action("List", "Servico")},
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoCadastroClientes, Label = "Clientes", OnClick = @Url.Action("List", "Cliente")},
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoCadastroResponsaveis, Label = "Responsáveis", OnClick = @Url.Action("List", "Responsavel")},
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.OrdemServicoConfiguracoes,
                    Label = "Configurações",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoConfiguracoesParametros, Label = "Parâmetros", OnClick = @Url.Action("List", "ParametroOrdemServico")}
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.OrdemServicoAjuda,
                    Label = "Ajuda",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.OrdemServicoAjudaAssistenciaRemota, Label =  "Assistência Remota", Link = "https://secure.logmeinrescue.com/customer/code.aspx"}
                    }
                },
                new SidebarUIMenu() { Class = ResourceHashConst.OrdemServicoAvalieAplicativo, Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") }
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