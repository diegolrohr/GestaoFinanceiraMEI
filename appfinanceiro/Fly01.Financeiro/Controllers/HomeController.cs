using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Config;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using System.Configuration;
using Fly01.uiJS.Classes.Widgets;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.uiJS.Enums;

namespace Fly01.Financeiro.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        protected override ContentUI HomeJson(bool withSidebarUrl = false)
        {
            ManagerEmpresaVM response = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
            var responseCidade = response.Cidade != null ? response.Cidade.Nome : string.Empty;

            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Fluxo de Caixa",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "save", Label = "Atualizar", OnClickFn = "fnAtualizar", Position = HtmlUIButtonPosition.Main },
                        new HtmlUIButton { Id = "prnt", Label = "Imprimir", OnClickFn = "fnImprimirFluxoCaixa", Position = HtmlUIButtonPosition.Out }
                    }
                },
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                Functions = new List<string> { "__format", "fnGetSaldos" }
            };

            if (withSidebarUrl)
                cfg.SidebarUrl = Url.Action("Sidebar", "Home", null, Request.Url.Scheme);

            var dataInicialFiltroDefault = DateTime.Now.Date;

            var dataFinal = DateTime.Now.AddMonths(1);
            var dataFinalFiltroDefault = new DateTime(dataFinal.Year, dataFinal.Month, DateTime.DaysInMonth(dataFinal.Year, dataFinal.Month));

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m8 offset-m2 printinfo",
                Color = "orange",
                Id = "fly01cardCabecalho",
                Placeholder = response.RazaoSocial + " | CNPJ: " + response.CNPJ +
                              " | Endereço: " + response.Endereco + ", "+ response.Numero +
                              " | Bairro: " + response.Bairro + " | CEP: " + response.CEP +
                              " | Cidade: " + responseCidade + " | Email: " + response.Email,
                Action = new LinkUI
                {
                    Label = "",
                    OnClick= ""
                }
            });

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
                            new ButtonGroupOptionUI { Id = "btnAno", Value = "6", Label = "Ano", Class = "col s4 m2" },
                            new ButtonGroupOptionUI { Id = "btnTri", Value = "4", Label = "Trimestre", Class = "col s6 m3" },
                            new ButtonGroupOptionUI { Id = "btnSem", Value = "5", Label = "Semestre", Class = "col s6 m3" },
                        }
                    }
                }
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = "orange",
                Id = "fly01cardSA",
                Title = "Saldo atual",
                Placeholder = "R$ 0,00",
                Action = new LinkUI
                {
                    Label = "Ver mais",
                    OnClick = @Url.Action("List", "Extrato")
                }
            });
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = "red",
                Id = "fly01cardCP",
                Title = "A pagar",
                Placeholder = "R$ 0,00",
                Action = new LinkUI
                {
                    Label = "Ver mais",
                    OnClick = @Url.Action("List", "ContaPagar")
                }
            });
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = "green",
                Id = "fly01cardCR",
                Title = "A receber",
                Placeholder = "R$ 0,00",
                Action = new LinkUI
                {
                    Label = "Ver mais",
                    OnClick = @Url.Action("List", "ContaReceber")
                }
            });
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = "blue",
                Id = "fly01cardSC",
                Title = "Saldo Projetado",
                Placeholder = "R$ 0,00",
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
                        text = "Fluxo de Caixa",
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
                UrlData = @Url.Action("LoadChart", "FluxoCaixa"),
                Class = "col s12",
                Parameters = new List<ChartUIParameter>
                {
                    new ChartUIParameter { Id = "dataInicial" },
                    new ChartUIParameter { Id = "dataFinal" },
                    new ChartUIParameter { Id = "groupType" }
                }
            });

            cfg.Content.Add(new DataTableUI
            {
                Id = "dtGridFluxoCaixa",
                Class = "col s12",
                UrlGridLoad = Url.Action("LoadGridFluxoCaixa", "FluxoCaixa"),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "dataInicial", Required = true },
                    new DataTableUIParameter { Id = "dataFinal", Required = true },
                    new DataTableUIParameter { Id = "groupType", Required = true }
                },
                Options = new DataTableUIConfig()
                {
                    PageLength = 10,
                    LengthChange = true
                },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn { DataField = "data", DisplayName = "Periodo", Priority = 1, Orderable = false, Searchable = false, Type = "date" },
                    new DataTableUIColumn { DataField = "totalRecebimentos", DisplayName = "Total Recebimentos", Priority = 3, Orderable = false, Searchable = false, Type = "currency" },
                    new DataTableUIColumn { DataField = "totalPagamentos", DisplayName = "Total Pagamentos", Priority = 4, Orderable = false, Searchable = false, Type = "currency" },
                    new DataTableUIColumn { DataField = "saldoFinal", DisplayName = "Saldo Final", Priority = 2, Orderable = false, Searchable = false, Type = "currency" }
                }
            });

            return cfg;
        }
        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Financeiro", Parent = "header" };

            #region MenuItems
            config.MenuItems.Add(new SidebarUIMenu()
            {
                Label = "Dashboard",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label = "Contas a Pagar", OnClick = @Url.Action("List", "DashboardContaPagar")},
                new LinkUI() { Label = "Contas a Receber", OnClick = @Url.Action("List", "DashboardContaReceber")},
            }
            });
            config.MenuItems.Add(new SidebarUIMenu()
            {
                Label = "Financeiro",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label = "Fluxo de Caixa", OnClick = @Url.Action("List", "Home")},
                new LinkUI() { Label = "Extrato", OnClick = @Url.Action("List", "Extrato")},
                new LinkUI() { Label = "Contas a Pagar", OnClick = @Url.Action("List", "ContaPagar")},
                new LinkUI() { Label = "Contas a Receber", OnClick = @Url.Action("List", "ContaReceber")},
                new LinkUI() { Label = "Relatório DRE", OnClick = @Url.Action("List", "DemonstrativoResultadoExercicio")},
                new LinkUI() { Label = "Conciliação Bancária", OnClick = @Url.Action("List", "ConciliacaoBancaria")},
            }
            });

            config.MenuItems.Add(new SidebarUIMenu()
            {
                Label = "Cobrança",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label = "Boletos", OnClick = @Url.Action("List", "Cnab")},
                new LinkUI() { Label = "Arquivos de remessa", OnClick = @Url.Action("List", "ArquivoRemessa")},
                new LinkUI() { Label = "Arquivo de retorno", OnClick = @Url.Action("Form", "ArquivoRetorno")}
            }
            });

            config.MenuItems.Add(new SidebarUIMenu()
            {
                Label = "Cadastros",
                Items = new List<LinkUI>
                {
                    new LinkUI() { Label = "Clientes",OnClick = @Url.Action("List", "Cliente")},
                    new LinkUI() { Label = "Fornecedores", OnClick = @Url.Action("List", "Fornecedor")},
                    new LinkUI() { Label = "Transportadoras", OnClick = @Url.Action("List", "Transportadora")},
                    new LinkUI() { Label = "Condições de Parcelamento",OnClick = @Url.Action("List", "CondicaoParcelamento")},
                    new LinkUI() { Label = "Categoria", OnClick = @Url.Action("List", "Categoria")},
                    new LinkUI() { Label = "Formas de Pagamento",OnClick = @Url.Action("List", "FormaPagamento")},
                    new LinkUI() { Label = "Contas Bancárias", OnClick = @Url.Action("List", "ContaBancaria")}
                }
            });

            config.MenuItems.Add(new SidebarUIMenu()
            {
                Label = "Configurações",
                Items = new List<LinkUI>
                {
                    new LinkUI() { Label = "Notificações", OnClick = @Url.Action("Form", "ConfiguracaoNotificacao")},
                }
            });

            config.MenuItems.Add(new SidebarUIMenu()
            {
                Label = "Ajuda",
                Items = new List<LinkUI>
                {
                    new LinkUI() { Label =  "Assistência Remota", Link = "https://secure.logmeinrescue.com/customer/code.aspx"}
                }
            });

            config.MenuItems.Add(new SidebarUIMenu() { Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp")});
            #endregion

            #region User Menu Items
            config.UserMenuItems.Add(new LinkUI() { Label = "Sair", Link = @Url.Action("Logoff", "Account") });
            #endregion

            #region Lista de aplicativos do usuário
            config.MenuApps.AddRange(AppsList());
            #endregion

            config.Name = SessionManager.Current.UserData.TokenData.Username;
            config.Email = SessionManager.Current.UserData.PlatformUser;

            config.Widgets = new WidgetsUI();
            config.Widgets.Conpass = new ConpassUI();
            config.Widgets.Droz = new DrozUI();
            config.Widgets.Zendesk = new ZendeskUI()
            {
                AppName = "Fly01 Financeiro",
                AppTag = "fly01_manufatura",
            };
            if (Request.Url.ToString().Contains("fly01.com.br"))
                config.Widgets.Insights = new InsightsUI { Key = ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"] };
            
            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}