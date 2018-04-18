using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.Core.Config;
using System.Collections.Generic;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;

namespace Fly01.Estoque.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        protected override ContentUI HomeJson(bool withSidebarUrl = false)
        {
            var dataInicialFiltroDefault = new DateTime(DateTime.Now.Year,
                                                        DateTime.Now.Month, 1)
                                                        .AddDays(-1);
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
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "atualizar",
                                           Label = "Atualizar",
                                           OnClickFn = "fnAtualizarPeriodo" },
                        new HtmlUIButton { Id = "posicaoatual",
                                           Label = "Posição Atual",
                                           OnClickFn = "fnPosicaoAtual" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            if (withSidebarUrl)
                cfg.SidebarUrl = Url.Action("Sidebar", "Home", null, Request.Url.Scheme);

            cfg.Content.Add(new FormUI
            {
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Class = "col s12",
                Functions = new List<string> { "fnAtualizarMaisMovimentados", "fnAtualizarMenosMovimentados", "fnAtualizarPeriodo" },
                Elements = new List<BaseUI>
                {
                    new InputDateUI { Id =  "dataInicial", Class = "col s6 m3 l4", Label = "Data Inicial", Value= dataInicialFiltroDefault.ToString("dd/MM/yyyy")},
                    new InputDateUI { Id =  "dataFinal", Class = "col s6 m3 l4", Label = "Data Final", Value = dataFinalFiltroDefault.ToString("dd/MM/yyyy")},
                    new ButtongroupUI()
                    {
                        Id = "fly01btngrp",
                        Class = "col s12 m6 l4",
                        Label = "Selecione o período",
                        OnClickFn = "fnAtualizarPeriodo",
                        Options = new List<OptionUI>
                        {
                            new OptionUI {Id = "btnDia", Value = "dia", Label = "Dia"},
                            new OptionUI {Id = "btnSemana", Value = "semana", Label = "Semana"},
                            new OptionUI {Id = "btnMes", Value = "mes", Label = "Mês"}
                        }
                    }
                }
            });

            cfg.Content.Add(new FormUI
            {
                Class = "col s12",
                Elements = new List<BaseUI>
                {
                    new LabelsetUI { Id =  "sss", Class = "col s12", Label = "Produtos"}
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
        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Estoque", Parent = "header" };

            #region MenuItems

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Estoque",
                Items = new List<LinkUI>
                {
                    new LinkUI() { Label = "Visão Geral", OnClick = @Url.Action("List")},
                    new LinkUI() { Label = "Ajuste Manual", OnClick = @Url.Action("Form", "AjusteManual")},
                    new LinkUI() { Label = "Posição Atual", OnClick = @Url.Action("List", "PosicaoAtual")},
                    new LinkUI() { Label = "Inventário", OnClick = @Url.Action("List", "Inventario")}
                }
            });

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Cadastros",
                Items = new List<LinkUI>
                {
                    new LinkUI() { Label = "Produtos", OnClick = @Url.Action("List", "Produto") },
                    new LinkUI() { Label = "Grupos de Produtos", OnClick = @Url.Action("List", "GrupoProduto") },
                    new LinkUI() { Label = "Tipos de Movimento", OnClick = @Url.Action("List", "TipoMovimento") }
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

            config.Zendesk = new ZendeskWidget()
            {
                AppName = "Fly01 Estoque",
                AppTag = "fly01_manufatura",
                Name = SessionManager.Current.UserData.TokenData.Username,
                Email = SessionManager.Current.UserData.PlatformUser
            };

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Default), "application/json");
        }
    }
}