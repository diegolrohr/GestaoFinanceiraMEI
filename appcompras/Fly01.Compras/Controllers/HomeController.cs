using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Compras.Controllers.Base;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.Core.Helpers;
using Fly01.Core.Config;
using Fly01.Core;
using Fly01.uiJS.Defaults;
using Fly01.Core.Api;

namespace Fly01.Compras.Controllers
{
    public class HomeController : GenericAppController
    {
        public override ActionResult Index()
        {
            return Request.IsAjaxRequest() ? Go() : base.Index();
        }

        [AllowCrossSiteJson]
        public ContentResult Go()
        {
            return Content(JsonConvert.SerializeObject(HomeJson(true), JsonSerializerSetting.Front), "application/json");
        }

        public ContentResult List()
        {
            return Content(JsonConvert.SerializeObject(HomeJson(), JsonSerializerSetting.Front), "application/json");
        }

        protected ContentUI HomeJson(bool withSidebarUrl = false)
        {
            return OrdemCompraController.OrdemCompraJson(Url, Request.Url.Scheme, withSidebarUrl);
        }

        public List<AppUI> AppsList()
        {
            List<AppUI> appsList = RestHelper.ExecuteGetRequest<List<AppUI>>(AppDefaults.UrlGateway.Replace("v2/compras", "v1"), String.Format("sidebarApps/{0}", SessionManager.Current.UserData.PlatformUrl), null);
            appsList.RemoveAll(x => x.Id == AppDefaults.AppIdCompras);
            string jwt = JWTHelper.Encode(new Dictionary<string, string>
                {
                    { "Email", SessionManager.Current.UserData.PlatformUser },
                    { "RazaoSocial", SessionManager.Current.UserData.TokenData.Username },
                    { "Fly01Url", SessionManager.Current.UserData.PlatformUrl }
                }, "http://gestao.fly01.com.br/", DateTime.Now.AddDays(1));

            foreach (var app in appsList.Where(x => x.Target.Go != null))
            {
                app.Target.Go = String.Format("{0}?t={1}", app.Target.Go, jwt);
            }

            return appsList;
        }

        public ContentResult Sidebar()
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