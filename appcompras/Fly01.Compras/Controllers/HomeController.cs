using Fly01.Core.Config;
using Fly01.Core.Presentation;
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
            if (!UserCanPerformOperation(ResourceHashConst.ComprasComprasDashboard))
                return new ContentUI();

            return DashboardController.DashboardJson(Url, Request.Url.Scheme, withSidebarUrl);
        }

        public override ContentResult Sidebar()
        {
            var config = new SidebarUI() { Id = "nav-bar", AppName = "Compras", Parent = "header" };

            #region MenuItems
            var menuItems = new List<SidebarUIMenu>()
            {
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.ComprasCompras,
                    Label = "Compras",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.ComprasComprasDashboard, Label = "Dashboard", OnClick = @Url.Action("List", "Dashboard")},
                        new LinkUI() { Class = ResourceHashConst.ComprasComprasOrcamentoPedido, Label = "Orçamentos/Pedidos", OnClick = @Url.Action("List", "OrdemCompra")},
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.ComprasCadastros,
                    Label = "Cadastros",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosProdutos, Label = "Produtos", OnClick = @Url.Action("List", "Produto")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosFornecedores, Label = "Fornecedores", OnClick = @Url.Action("List", "Fornecedor")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosTransportadora, Label = "Transportadoras", OnClick = @Url.Action("List", "Transportadora")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosCondicoesParcelamento, Label = "Condições de Parcelamento",OnClick = @Url.Action("List", "CondicaoParcelamento")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosFormaPagamento, Label = "Forma de Pagamento", OnClick = @Url.Action("List", "FormaPagamento")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosGrupoTributario, Label = "Grupo Tributário", OnClick = @Url.Action("List", "GrupoTributario")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosGrupoProdutos, Label = "Grupo de Produtos", OnClick = @Url.Action("List", "GrupoProduto")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosCategoria, Label = "Categoria", OnClick = @Url.Action("List", "Categoria")},
                        new LinkUI() { Class = ResourceHashConst.ComprasCadastrosSubstituicaoTributaria, Label = "Substituição Tributária", OnClick = @Url.Action("List", "SubstituicaoTributaria")}
                    }
                },
                new SidebarUIMenu()
                {
                    Class = ResourceHashConst.ComprasAjuda,
                    Label = "Ajuda",
                    Items = new List<LinkUI>
                    {
                        new LinkUI() { Class = ResourceHashConst.ComprasAjudaAssistenciaRemota, Label =  "Assistência Remota", Link = "https://secure.logmeinrescue.com/customer/code.aspx"}
                    }
                },
                new SidebarUIMenu() { Class = ResourceHashConst.ComprasAvalieAplicativo, Label = "Avalie o Aplicativo", OnClick = @Url.Action("List", "AvaliacaoApp") }
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
                    AppName = "Fly01 Compras",
                    AppTag = "fly01_manufatura",
                }
            };
            if (Request.Url.ToString().Contains("fly01.com.br"))
                config.Widgets.Insights = new InsightsUI { Key = ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"] };

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}