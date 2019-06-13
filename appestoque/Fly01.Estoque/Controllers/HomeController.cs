using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.Core.Config;
using System.Collections.Generic;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using System.Configuration;
using Fly01.uiJS.Classes.Widgets;
using Fly01.Core.Presentation;
using Fly01.uiJS.Enums;
using Fly01.Core;
using Fly01.Core.Helpers;

namespace Fly01.Estoque.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        private List<HtmlUIButton> GetButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>
            {
                new HtmlUIButton() { Id = "atualizar", Label = "Atualizar", OnClickFn = "fnAtualizarPeriodo", Position = HtmlUIButtonPosition.Main }
            };

            if (UserCanPerformOperation(ResourceHashConst.EstoqueEstoquePosicaoAtual))
                target.Add( new HtmlUIButton() { Id = "posicaoatual", Label = "Posição Atual", OnClickFn = "fnPosicaoAtual", Position = HtmlUIButtonPosition.Out });

            return target;
        }

        protected override ContentUI HomeJson()
        {
            if (!UserCanPerformOperation(ResourceHashConst.EstoqueEstoqueVisaoGeral))
                return new ContentUI { SidebarUrl = @Url.Action("Sidebar") };

            var dataInicialFiltroDefault = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            var dataFinalFiltroDefault = DateTime.Now.Date;

            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Visão Geral",
                    Buttons = new List<HtmlUIButton>(GetButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                SidebarUrl = @Url.Action("Sidebar")
            };

            cfg.Content.Add(new FormUI
            {
                Id = "fly01frm",
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Class = "col s12",
                Functions = new List<string> { "fnAtualizarMaisMovimentados", "fnAtualizarMenosMovimentados", "fnAtualizarPeriodo" },
                Elements = new List<BaseUI>
                {
                    new InputDateUI { Id =  "dataInicial", Class = "col s6 m3 l4", Label = "Data Inicial", Value= dataInicialFiltroDefault.ToString("dd/MM/yyyy")},
                    new InputDateUI { Id =  "dataFinal", Class = "col s6 m3 l4", Label = "Data Final", Value = dataFinalFiltroDefault.ToString("dd/MM/yyyy")},
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

            cfg.Content.Add(new FormUI
            {
                Id = "fly01frm",
                Class = "col s12",
                Elements = new List<BaseUI>
                {
                    new LabelSetUI { Id =  "sss", Class = "col s12", Label = "Produtos"}
                }

            });

            cfg.Content.Add(new DataTableUI
            {
                Id = "cardProdutosMaisMovimentados",
                Class = "col s12 m6",
                UrlGridLoad = Url.Action("GridLoadProdutosMaisMovimentados", "ProdutosMaisMovimentados"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnExibeDatatableMaisMovimentados" },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn
                    {
                        DataField = "Id",
                        DisplayName = "Mais movimentados",
                        Orderable = false,
                        Searchable = false,
                        RenderFn = "fnExibeDatatableMaisMovimentados"
                    }
                },
                Options = new DataTableUIConfig
                {
                    WithoutRowMenu = true,
                    PageLength = 6
                },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "dataInicial" },
                    new DataTableUIParameter { Id = "dataFinal" }
                }
            });

            cfg.Content.Add(new DataTableUI
            {
                Id = "cardProdutosMenosMovimentados",
                Class = "col s12 m6",
                UrlGridLoad = Url.Action("GridLoadProdutosMenosMovimentados", "ProdutosMenosMovimentados"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnExibeDatatableMenosMovimentados" },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn
                    {
                        DataField = "Id",
                        DisplayName = "Menos movimentados",
                        Orderable = false,
                        Searchable = false,
                        RenderFn = "fnExibeDatatableMenosMovimentados"
                    }
                },
                Options = new DataTableUIConfig
                {
                    WithoutRowMenu = true,
                    PageLength = 6
                },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "dataInicial" },
                    new DataTableUIParameter { Id = "dataFinal" }
                }
            });

            cfg.Content.Add(new DataTableUI
            {
                Id = "cardProdutosSaldoZerado",
                Class = "col s12 m6",
                UrlGridLoad = @Url.Action("GridLoadSaldoZerado", "Produto"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnExibeDatatableSaldoZerado" },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn
                    {
                        DataField = "Id",
                        DisplayName = "Saldo zerado",
                        Orderable = false,
                        Searchable = false,
                        RenderFn = "fnExibeDatatableSaldoZerado"
                    }
                },
                Options = new DataTableUIConfig
                {
                    WithoutRowMenu = true,
                    PageLength = 5
                }
            });

            cfg.Content.Add(new DataTableUI
            {
                Id = "cardProdutosSaldoAbaixoMinimo",
                Class = "col s12 m6",
                UrlGridLoad = Url.Action("GridLoadSaldoAbaixoMinimo", "Produto"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnExibeDatatableSaldoAbaixoMinimo" },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn
                    {
                        DataField = "Id",
                        DisplayName = "Saldo abaixo do mínimo",
                        Orderable = false,
                        Searchable = false,
                        RenderFn = "fnExibeDatatableSaldoAbaixoMinimo"
                    }
                },
                Options = new DataTableUIConfig
                {
                    WithoutRowMenu = true,
                    PageLength = 5
                }
            });

            return cfg;
        }

        private string GenerateJWT()
        {
            var payload = new Dictionary<string, string>()
                {
                    {  "platformUrl", SessionManager.Current.UserData.PlatformUrl },
                    {  "clientId", AppDefaults.AppId },
                };
            var token = JWTHelper.Encode(payload, "https://meu.bemacash.com.br/", DateTime.Now.AddMinutes(60));
            return token;
        }

        public JsonResult NotificationJwt()
        {
            return Json(new
            {
                token = GenerateJWT()
            }, JsonRequestBehavior.AllowGet);
        }

        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Estoque", Parent = "header" };

            #region MenuItems
            var menuItems = new List<SidebarUIMenu>()
            {
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.EstoqueEstoque,
                    Label = "Estoque",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.EstoqueEstoqueVisaoGeral, Label = "Visão Geral", OnClick = @Url.Action("List")},
                        new LinkUI() { Class = ResourceHashConst.EstoqueEstoqueAjusteManual, Label = "Ajuste Manual", OnClick = @Url.Action("Form", "AjusteManual")},
                        new LinkUI() { Class = ResourceHashConst.EstoqueEstoquePosicaoAtual, Label = "Posição Atual", OnClick = @Url.Action("List", "PosicaoAtual")},
                        new LinkUI() { Class = ResourceHashConst.EstoqueEstoqueInventario, Label = "Inventário", OnClick = @Url.Action("List", "Inventario")},
                        new LinkUI() { Class = ResourceHashConst.EstoqueEstoqueRelatorios, Label = "Relatórios", OnClick = @Url.Action("List", "Relatorio")}
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.EstoqueCadastros,
                    Label = "Cadastros",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.EstoqueCadastrosProdutos, Label = "Produtos", OnClick = @Url.Action("List", "Produto") },
                        new LinkUI() { Class = ResourceHashConst.EstoqueCadastrosGrupoProdutos, Label = "Grupos de Produtos", OnClick = @Url.Action("List", "GrupoProduto") },
                        new LinkUI() { Class = ResourceHashConst.EstoqueCadastrosTiposMovimento, Label = "Tipos de Movimento", OnClick = @Url.Action("List", "TipoMovimento") }
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.EstoqueConfiguracoes,
                    Label = "Configurações",
                    Items = new List<LinkUI>
                    {
                        //Personalizar Sistema não vai ter hash especifico de permissão, segundo Fraga
                        new LinkUI() { Class = ResourceHashConst.EstoqueConfiguracoes, Label = "Personalizar Sistema", OnClick = @Url.Action("Form", "ConfiguracaoPersonalizacao") },
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.EstoqueAjuda,
                    Label = "Ajuda",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.EstoqueAjuda, Label = "Manual do Usuário", Link = "https://centraldeatendimento.totvs.com/hc/pt-br/categories/360000364572" },
                        new LinkUI() { Class = ResourceHashConst.EstoqueAjudaAssistenciaRemota, Label =  "Assistência Remota", OnClick = @Url.Action("Form", "AssistenciaRemota") }
                    }
                },
                new SidebarUIMenu() { Class = ResourceHashConst.EstoqueAvalieAplicativo, Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") }
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

            config.Name = SessionManager.Current.UserData.TokenData.Username;
            config.Email = SessionManager.Current.UserData.PlatformUser;
            config.Notification = new SidebarUINotification()
            {
                Channel = AppDefaults.AppId + "_" + SessionManager.Current.UserData.PlatformUrl,
                JWT = @Url.Action("NotificationJwt"),
                SocketServer = AppDefaults.UrlNotificationSocket
            };

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

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Default), "application/json");
        }
    }
}