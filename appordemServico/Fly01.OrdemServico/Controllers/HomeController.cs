using Fly01.Core.Config;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
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
            //if (!UserCanPerformOperation(ResourceHashConst.FinanceiroFinanceiroFluxoCaixa))
            //    return new ContentUI{ SidebarUrl = @Url.Action("Sidebar") };

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
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns="
            };
            var dataInicialFiltroDefault = DateTime.Now.Date;

            var dataFinal = DateTime.Now.AddMonths(1);
            var dataFinalFiltroDefault = new DateTime(dataFinal.Year, dataFinal.Month, DateTime.DaysInMonth(dataFinal.Year, dataFinal.Month));

            cfg.Content.Add(new FormUI
            {
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
                Color = "totvs-blue",
                Id = "fly01cardAB",
                Title = "Em Aberto",
                Placeholder = "0/0",
                Action = new LinkUI
                {
                    Label = "Ver mais",
                    OnClick = @Url.Action("List", "OrdemServico")
                }
            });
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = "red",
                Id = "fly01cardAN",
                Title = "Em Andamento",
                Placeholder = "0/0",
                Action = new LinkUI
                {
                    Label = "Ver mais",
                    OnClick = @Url.Action("List", "OrdemServico")
                }
            });
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = "green",
                Id = "fly01cardCO",
                Title = "Concluído",
                Placeholder = "0/0",
                Action = new LinkUI
                {
                    Label = "Ver mais",
                    OnClick = @Url.Action("List", "OrdemServico")
                }
            });
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = "teal",
                Id = "fly01cardCA",
                Title = "Cancelado",
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
                UrlData = @Url.Action("LoadChart", "Dashboard"),
                Class = "col s12",
                Parameters = new List<ChartUIParameter>
                {
                    new ChartUIParameter { Id = "dataInicial" },
                    new ChartUIParameter { Id = "dataFinal" },
                    new ChartUIParameter { Id = "groupType" }
                }
            });

            cfg.Content.Add(new DivUI
            {
                Elements = new List<BaseUI>
                {
                    new LabelSetUI { Id = "t1", Class = "col s6", Label = "Os 10 maiores produtos" },
                    new LabelSetUI { Id = "t2", Class = "col s6", Label = "Os 10 maiores serviços" }
                }
            });
            cfg.Content.Add(new DataTableUI
            {
                Class = "col s6",
                Id = "dtGridTopProdutos",
                UrlGridLoad = Url.Action("DashboardTopProdutos", "OrdemServico"),
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
                Id = "dtGridTopServicos",
                UrlGridLoad = Url.Action("DashboardTopServicos", "OrdemServico"),
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

            if (withSidebarUrl)
                cfg.SidebarUrl = Url.Action("Sidebar", "Home", null, Request.Url.Scheme);

            return cfg;
        }

        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Ordem de Serviço", Parent = "header" };

            #region MenuItems
            var menuItems = new List<SidebarUIMenu>()
            {
                new SidebarUIMenu()
                {
                    //Class = ResourceHashConst.FinanceiroFinanceiro,
                    Label = "Ordem de Serviço",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FinanceiroFinanceiroFluxoCaixa, Label = "Visão Geral", OnClick = @Url.Action("List", "Home")},
                        new LinkUI() { Class = ResourceHashConst.FinanceiroFinanceiroFluxoCaixa, Label = "Ordem de Serviço", OnClick = @Url.Action("List", "OrdemServico")},
                    }
                },
                new SidebarUIMenu()
                {
                    //Class = ResourceHashConst.FinanceiroFinanceiro,
                    Label = "Cadastro",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FinanceiroFinanceiroFluxoCaixa, Label = "Produtos", OnClick = @Url.Action("List", "Produto")},
                        new LinkUI() { Class = ResourceHashConst.FinanceiroFinanceiroFluxoCaixa, Label = "Clientes", OnClick = @Url.Action("List", "Cliente")},
                        new LinkUI() { Class = ResourceHashConst.FinanceiroFinanceiroFluxoCaixa, Label = "Responsáveis", OnClick = @Url.Action("List", "Responsavel")},
                    }
                },
                new SidebarUIMenu()
                {
                    //Class = ResourceHashConst.FinanceiroConfiguracoes,
                    Label = "Configurações",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FinanceiroConfiguracoesNotificacoes, Label = "Parâmetros", OnClick = @Url.Action("List", "ParametroOrdemServico")}
                    }
                },
                new SidebarUIMenu()
                {
                    //Class = ResourceHashConst.FinanceiroAjuda,
                    Label = "Ajuda",
                    Items = new List<LinkUI>
                    {
                        //new LinkUI() { Class = ResourceHashConst.OrdemServicoAjudaAssistenciaRemota, Label =  "Assistência Remota", Link = "https://secure.logmeinrescue.com/customer/code.aspx"}
                        new LinkUI() { Class = ResourceHashConst.FinanceiroAjudaAssistenciaRemota, Label =  "Assistência Remota", Link = "https://secure.logmeinrescue.com/customer/code.aspx"}
                    }
                },
                //TODO: Ver permissoes new SidebarUIMenu() { Class = ResourceHashConst.OrdemServicoAvalieAplicativo, Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") }
                new SidebarUIMenu() { Class = "", Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") }
            };

            //config.MenuItems.AddRange(ProcessMenuRoles(menuItems));
            config.MenuItems.AddRange(menuItems);
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

        public JsonResult LoadChart(DateTime dataInicial, DateTime dataFinal, int groupType)
        {
            try
            {
                //var response = GetProjecao(dataInicial, dataFinal, groupType);

                var dataChartToView = new
                {
                    success = true,
                    currency = true,
                    //labels = response.Select(x => x.Label).ToArray(),
                    //datasets = new object[] {
                    //    new {
                    //            type = "line",
                    //            label = "Saldo",
                    //            backgroundColor = "rgb(44, 55, 57)",
                    //            borderColor = "rgb(44, 55, 57)",
                    //            //data = response.Select(x => Math.Round(x.SaldoFinal, 2)).ToArray(),
                    //            fill = false
                    //        },
                    //    new {
                    //            label = "Recebimentos",
                    //            fill = false,
                    //            backgroundColor = "rgb(0, 178, 121)",
                    //            borderColor = "rgb(0, 178, 121)",
                    //            //data = response.Select(x => Math.Round(x.TotalRecebimentos, 2)).ToArray()
                    //        },
                    //    new {
                    //            label = "Pagamentos",
                    //            fill = false,
                    //            backgroundColor = "rgb(239, 100, 97)",
                    //            borderColor = "rgb(239, 100, 97)",
                    //            //data = response.Select(x => Math.Round(x.TotalPagamentos * -1, 2)).ToArray()
                    //    }
                    //}
                };

                return Json(dataChartToView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }


    }

}