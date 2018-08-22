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
        protected override ContentUI HomeJson(bool withSidebarUrl = false)
        {
            //if (!UserCanPerformOperation(ResourceHashConst.FinanceiroFinanceiroFluxoCaixa))
            //    return new ContentUI();

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
                    new InputDateUI
                    {
                        Id = "dataInicial",
                        Class = "col s6 m3 l4 offset-l2 offset-m3",
                        Label = "Data Inicial",
                        Value = dataInicialFiltroDefault.ToString("dd/MM/yyyy"),
                        DomEvents = new List<DomEventUI>
                        {
                            new DomEventUI {DomEvent = "click", Function = "fnAtualizar"}
                        },
                        Max = 90,
                        Min = true
                    },
                    new InputDateUI
                    {
                        Id = "dataFinal",
                        Class = "col s6 m3 l4",
                        Label = "Data Final",
                        Value = dataFinalFiltroDefault.ToString("dd/MM/yyyy"),
                        DomEvents = new List<DomEventUI> {new DomEventUI {DomEvent = "click", Function = "fnAtualizar"}},
                        Max = 90,
                        Min = true
                    },
                    new InputHiddenUI{ Id = "groupType", Value = "1" },
                    new ButtonGroupUI()
                    {
                        Id = "fly01btngrp",
                        Class = "col s12 hide-on-print",
                        Label = "Tipo de Visualização",
                        OnClickFn = "fnAtualizaAgrupamento",
                        Options = new List<ButtonGroupOptionUI>
                        {
                            new ButtonGroupOptionUI { Id = "btnDia", Value = "1", Label = "Dia", Class = "col s4 m2" },
                            new ButtonGroupOptionUI { Id = "btnMes", Value = "3", Label = "Mês", Class = "col s4 m2" },
                            new ButtonGroupOptionUI { Id = "btnTri", Value = "4", Label = "Trimestre", Class = "col s6 m3" },
                            new ButtonGroupOptionUI { Id = "btnSem", Value = "5", Label = "Semestre", Class = "col s6 m3" },
                            new ButtonGroupOptionUI { Id = "btnAno", Value = "6", Label = "Ano", Class = "col s4 m2" },
                        }
                    }
                }
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = "totvs-blue",
                Id = "cardAberto",
                Title = "Em Aberto",
                Placeholder = "0/100",
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
                Id = "cardAndamento",
                Title = "Em Andamento",
                Placeholder = "0/100",
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
                Id = "cardConcluido",
                Title = "Concluído",
                Placeholder = "0/100",
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
                Id = "cardCancelado",
                Title = "Cancelado",
                Placeholder = "0/100",
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
                UrlData = @Url.Action("LoadChart", "Home"),
                Class = "col s12",
                Parameters = new List<ChartUIParameter>
                {
                    new ChartUIParameter { Id = "dataInicial" },
                    new ChartUIParameter { Id = "dataFinal" },
                    new ChartUIParameter { Id = "groupType" }
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