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
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                SidebarUrl = @Url.Action("Sidebar")
                //    //Header = new HtmlUIHeader
                //    //{
                //    //    Title = "Fluxo de Caixa",
                //    //    Buttons = new List<HtmlUIButton>
                //    //    {
                //    //        new HtmlUIButton { Id = "save", Label = "Atualizar", OnClickFn = "fnAtualizar", Position = HtmlUIButtonPosition.Main },
                //    //        new HtmlUIButton { Id = "prnt", Label = "Imprimir", OnClickFn = "fnImprimirFluxoCaixa", Position = HtmlUIButtonPosition.Out }
                //    //    }
                //    //},
                //    //UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                //    //Functions = new List<string> { "__format", "fnGetSaldos" }
            };

            return cfg;
        }

        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Ordem Serviço", Parent = "header" };

            #region MenuItems
            var menuItems = new List<SidebarUIMenu>()
            {
                new SidebarUIMenu()
                {
                    //Class = ResourceHashConst.FinanceiroFinanceiro,
                    Label = "Cadastro",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FinanceiroFinanceiroFluxoCaixa, Label = "SubMenu1", OnClick = @Url.Action("List", "Home")},
                        new LinkUI() { Class = ResourceHashConst.FinanceiroFinanceiroFluxoCaixa, Label = "SubMenu2", OnClick = @Url.Action("List", "Home")},
                    }
                },
                new SidebarUIMenu()
                {
                    //Class = ResourceHashConst.FinanceiroConfiguracoes,
                    Label = "Configurações",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.FinanceiroConfiguracoesNotificacoes, Label = "Notificações", OnClick = @Url.Action("List", "Home")}                        
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
    }
}