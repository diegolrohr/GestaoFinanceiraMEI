using Fly01.Core.Config;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Widgets;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        protected override ContentUI HomeJson(bool withSidebarUrl = false)
        {
            //return OrdemCompraController.OrdemCompraJson(Url, Request.Url.Scheme, withSidebarUrl);
            return DashboardController.DashboardJson(Url, Request.Url.Scheme, withSidebarUrl);
        }
        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Compras", Parent = "header" };

            #region MenuItems

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Compras",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label = "Dashboard", OnClick = @Url.Action("List", "Dashboard")},
                new LinkUI() { Label = "Orçamentos/Pedidos", OnClick = @Url.Action("List", "OrdemCompra")},
            }
            });

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Cadastros",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label = "Produtos", OnClick = @Url.Action("List", "Produto")},
                new LinkUI() { Label = "Fornecedores", OnClick = @Url.Action("List", "Fornecedor")},
                new LinkUI() { Label = "Condições de Parcelamento",OnClick = @Url.Action("List", "CondicaoParcelamento")},
                new LinkUI() { Label = "Forma de Pagamento", OnClick = @Url.Action("List", "FormaPagamento")},
                new LinkUI() { Label = "Grupo Tributário", OnClick = @Url.Action("List", "GrupoTributario")},
                new LinkUI() { Label = "Grupo de Produtos", OnClick = @Url.Action("List", "GrupoProduto")},
                new LinkUI() { Label = "Categoria", OnClick = @Url.Action("List", "Categoria")},
                new LinkUI() { Label = "Substituição Tributária", OnClick = @Url.Action("List", "SubstituicaoTributaria")}
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

            config.MenuItems.Add(new SidebarMenuUI() { Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") });

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
                AppName = "Fly01 Compras",
                AppTag = "fly01_manufatura",
            };
            if (Request.Url.ToString().Contains("fly01.com.br"))
                config.Widgets.Insights = new InsightsUI { Key = ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"] };

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}