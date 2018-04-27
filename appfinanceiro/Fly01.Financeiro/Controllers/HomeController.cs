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
using Fly01.Core.Reports;
using Fly01.Core;
using Fly01.Core.Rest;

namespace Fly01.Financeiro.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        protected override ContentUI HomeJson(bool withSidebarUrl = false)
        {
            ManagerEmpresaVM response = RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{SessionManager.Current.UserData.PlatformUrl}");

            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Fluxo de Caixa",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "save", Label = "Atualizar", OnClickFn = "fnAtualizar" },
                        new HtmlUIButton { Id = "print", Label = "Imprimir", OnClickFn = "fnImprimirFluxoCaixa" }
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

            cfg.Content.Add(new DivUI
            {
                Class = "col s12 m12 printinfo",
                Id = "fly01cardCabecalho",
                Elements = new List<BaseUI>
                {
                    new StatictextUI
                    {
                        Class= "col s12",
                        Lines = new List<LineUI>{
                            new LineUI {Class = "cabecalho05", Tag = "p", Text = response.RazaoSocial + " | " + "CNPJ: " + response.CNPJ  },
                            new LineUI {Class = "cabecalho05", Tag = "p", Text = "Endereço: " + response.Endereco + ", " + response.Numero },
                            new LineUI {Class = "cabecalho05", Tag = "p", Text ="Bairro: " + response.Bairro + " | " + "CEP: " + response.CEP },
                            new LineUI {Class = "cabecalho05", Tag = "p", Text = "Cidade: " + response.Cidade.Nome + " | " + "Email: " + response.Email },
                            new LineUI {Class = "cabecalho05", Tag = "p", Text = " " }
                        }
                    }
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
                        Class = "col s6 m3 l4",
                        Label = "Data Inicial",
                        Value = dataInicialFiltroDefault.ToString("dd/MM/yyyy"),
                        DomEvents = new List<DomEventUI>
                        {
                            new DomEventUI {DomEvent = "click", Function = "fnAtualizar"}
                        },
                        Max = 60,
                        Min = true
                    },
                    new InputDateUI
                    {
                        Id = "dataFinal",
                        Class = "col s6 m3 l4",
                        Label = "Data Final",
                        Value = dataFinalFiltroDefault.ToString("dd/MM/yyyy"),
                        DomEvents = new List<DomEventUI> {new DomEventUI {DomEvent = "click", Function = "fnAtualizar"}},
                        Max = 60,
                        Min = true
                    },
                    new ButtongroupUI()
                    {
                        Id = "fly01btngrp",
                        Class = "col s12 m6 l4",
                        Label = "Selecione o período",
                        OnClickFn = "fnAtualizarPeriodo",
                        Options = new List<OptionUI>
                        {
                            new OptionUI {Id = "btnDia", Value = "1", Label = "Dia"},
                            new OptionUI {Id = "btnSemana", Value = "7", Label = "Semana"},
                            new OptionUI {Id = "btnMes", Value = "30", Label = "Mês"}
                        }
                    }
                }

            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m3",
                Color = "orange",
                Id = "fly01cardSaldo",
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
                        new ChartUIParameter { Id = "dataFinal" }
                    }

            });

            cfg.Content.Add(new DataTableUI
            {
                Class = "col s12",
                UrlGridLoad = Url.Action("LoadGridFluxoCaixa", "FluxoCaixa"),
                Parameters = new List<DataTableUIParameter>
                    {
                        new DataTableUIParameter { Id = "dataInicial" },
                        new DataTableUIParameter { Id = "dataFinal" }
                    },
                Options = new DataTableUIConfig()
                {
                    PageLength = 10
                },
                Columns = new List<DataTableUIColumn>
                    {
                        new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 1, Orderable = false, Searchable = false, Type = "date" },
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
            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Financeiro",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label = "Fluxo de Caixa", OnClick = @Url.Action("List", "Home")},
                new LinkUI() { Label = "Extrato", OnClick = @Url.Action("List", "Extrato")},
                new LinkUI() { Label = "Contas a Pagar", OnClick = @Url.Action("List", "ContaPagar")},
                new LinkUI() { Label = "Contas a Receber", OnClick = @Url.Action("List", "ContaReceber")},
                new LinkUI() { Label = "Relatório DRE", OnClick = @Url.Action("List", "DemonstrativoResultadoExercicio")},
                //new LinkUI() { Label = "Borderôs/CNAB", OnClick = @Url.Action("CNAB", "Json")},
                new LinkUI() { Label = "Conciliação Bancária", OnClick = @Url.Action("List", "ConciliacaoBancaria")},
            }
            });

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Cadastros",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label = "Clientes",OnClick = @Url.Action("List", "Cliente")},
                new LinkUI() { Label = "Fornecedores", OnClick = @Url.Action("List", "Fornecedor")},
                new LinkUI() { Label = "Condições de Parcelamento",OnClick = @Url.Action("List", "CondicaoParcelamento")},
                new LinkUI() { Label = "Categoria", OnClick = @Url.Action("List", "Categoria")},
                new LinkUI() { Label = "Formas de Pagamento",OnClick = @Url.Action("List", "FormaPagamento")},
                new LinkUI() { Label = "Contas Bancárias", OnClick = @Url.Action("List", "ContaBancaria")}
            }
            });

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Configurações",
                Items = new List<LinkUI>
                {
                    new LinkUI() { Label = "Notificações", OnClick = @Url.Action("Form", "ConfiguracaoNotificacao")},
                }
            });

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Ajuda",
                Items = new List<LinkUI>
                {
                    new LinkUI() { Label =  "Assistência Remota", Link = "https://secure.logmeinrescue.com/customer/code.aspx"}
                }
            });
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