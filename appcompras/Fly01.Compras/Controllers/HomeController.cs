using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.Core.Config;
using Fly01.uiJS.Defaults;

namespace Fly01.Compras.Controllers
{
    public class HomeController : Core.Presentation.Controllers.HomeController
    {
        protected override ContentUI HomeJson(bool withSidebarUrl = false)
        {
            return OrdemCompraController.OrdemCompraJson(Url, Request.Url.Scheme, withSidebarUrl, "GridLoad");
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
                new LinkUI() { Label = "Orçamento/Pedido", OnClick = @Url.Action("List", "Home")},
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

            #endregion

            #region User Menu Items
            config.UserMenuItems.Add(new LinkUI() { Label = "Sair", Link = @Url.Action("Logoff", "Account") });
            #endregion

            #region Lista de aplicativos do usuário
            config.MenuApps.AddRange(AppsList());
            #endregion

            config.Zendesk = new ZendeskWidget()
            {
                AppName = "Fly01 Compras",
                AppTag = "fly01_manufatura",
                Name = SessionManager.Current.UserData.TokenData.Username,
                Email = SessionManager.Current.UserData.PlatformUser
            };

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}