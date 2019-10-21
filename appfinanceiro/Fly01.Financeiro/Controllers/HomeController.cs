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
using Fly01.Core.Presentation;
using Fly01.Core;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        public override ActionResult Index()
        {
            return RedirectToAction("Index", "ContaPagar");
        }

        protected override ContentUI HomeJson()
        {
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
                Functions = new List<string> { "__format", "fnGetSaldos" },
                SidebarUrl = Url.Action("Sidebar", "Home")
            };

            var dataInicialFiltroDefault = DateTime.Now.Date;

            var dataFinal = DateTime.Now.AddMonths(1);
            var dataFinalFiltroDefault = new DateTime(dataFinal.Year, dataFinal.Month, DateTime.DaysInMonth(dataFinal.Year, dataFinal.Month));

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m8 offset-m2 printinfo",
                Color = "totvs-blue",
                Id = "fly01cardCabecalho",
                Placeholder = "Empresa XYZ",
                Action = new LinkUI
                {
                    Label = "",
                    OnClick = ""
                }
            });

            cfg.Content.Add(new FormUI
            {
                Id = "fly01frm",
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
                        //Max = 90,
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
                Color = "teal",
                Id = "fly01cardSC",
                Title = "Saldo projetado",
                Placeholder = "R$ 0,00",
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
                    LengthChange = true,
                    NoExportButtons = true
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
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Gestão Financeira MEI", Parent = "header" };
            config.Images = new SidebarUILogos()
            {
                Main = " "//TODO: Trocar Logo
            };

            var financeiroMenuItens = new SidebarUIMenu()
            {
                Label = "Financeiro",
                Items = new List<LinkUI>
                    {
                        //new LinkUI() { Label = "Fluxo de Caixa", OnClick = @Url.Action("List", "Home")},
                        new LinkUI() { Label = "Fluxo de Caixa", OnClick = @Url.Action("List", "ContaPagar1")},
                        //new LinkUI() { Label = "Extrato", OnClick = @Url.Action("List", "Extrato")},
                        new LinkUI() { Label = "Contas a Pagar", OnClick = @Url.Action("List", "ContaPagar")},
                        new LinkUI() { Label = "Contas a Receber", OnClick = @Url.Action("List", "ContaReceber")},
                        new LinkUI() { Label = "Relatório DRE", OnClick = @Url.Action("List", "DemonstrativoResultadoExercicio")},
                        //new LinkUI() { Label = "Relatórios", OnClick = @Url.Action("List", "Relatorio")},
                        new LinkUI() { Label = "Conciliação Bancária", OnClick = @Url.Action("List", "ConciliacaoBancaria")}
                    }
            };

            #region MenuItems
            var menuItems = new List<SidebarUIMenu>()
            {
                //new SidebarUIMenu()
                //{
                //    Label = "Indicadores",
                //    Items = new List<LinkUI>
                //    {
                //        new LinkUI() { Label = "Contas a Pagar", OnClick = @Url.Action("List", "DashboardContaPagar")},
                //        new LinkUI() { Label = "Contas a Receber", OnClick = @Url.Action("List", "DashboardContaReceber")},
                //    }
                //},

                financeiroMenuItens,

                new SidebarUIMenu()
                {
                    Label = "Cadastros",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Label = "Clientes",OnClick = @Url.Action("List", "Cliente")},
                        new LinkUI() { Label = "Fornecedores", OnClick = @Url.Action("List", "Fornecedor")},
                        new LinkUI() { Label = "Condições de Parcelamento",OnClick = @Url.Action("List", "CondicaoParcelamento")},
                        new LinkUI() { Label = "Categoria", OnClick = @Url.Action("List", "Categoria")},
                        new LinkUI() { Label = "Formas de Pagamento",OnClick = @Url.Action("List", "FormaPagamento")},
                        new LinkUI() { Label = "Contas Bancárias", OnClick = @Url.Action("List", "ContaBancaria")},
                    }
                }
            };
            config.MenuItems.AddRange(menuItems);
            #endregion

            config.Name = "Usuário Teste";
            config.Email = "teste@gmail.com";

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}