using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Core.Config;
using Fly01.Core.Rest;
using Fly01.uiJS.Classes.Elements;
using System.Linq;
using Fly01.Core.Helpers;
using System;
using Fly01.Core.Presentation.Commons;

namespace Fly01.Core.Presentation.Controllers
{
    [OperationRole(NotApply = true)]
    public abstract class HomeController : GenericAppController
    {
        public override ActionResult Index()
            => Request.IsAjaxRequest() ? Go() : base.Index();

        public ContentResult Go()
            => Content(JsonConvert.SerializeObject(HomeJson(), JsonSerializerSetting.Front), "application/json");

        public ContentResult List()
            => Content(JsonConvert.SerializeObject(HomeJson(), JsonSerializerSetting.Front), "application/json");

        public List<AppUI> AppsList()
        {
            var requestObject = new { platformUrl = SessionManager.Current.UserData.PlatformUrl, platformUser = SessionManager.Current.UserData.PlatformUser, originApp = AppDefaults.AppId };
            return RestHelper.ExecutePostRequest<List<AppUI>>(AppDefaults.UrlGateway, "v1/sidebarApps", requestObject);
        }

        protected abstract ContentUI HomeJson();

        public abstract ContentResult Sidebar();

        public virtual List<SidebarUIMenu> ProcessMenuRoles(List<SidebarUIMenu> appMenu)
        {
            var resourceByUser = SessionManager.Current.UserData.Permissions.Select(x => x.ResourceHash.ToUpper()).ToList();

            var subMenuItems = appMenu.SelectMany(x => x.Items).Where(x => !resourceByUser.Contains(x.Class.ToUpper())).Select(x => x.Class);
            var menuItems = appMenu.Where(x => !resourceByUser.Contains(x.Class.ToUpper())).Select(x => x.Class);
            var itemsToRemove = subMenuItems.Union(menuItems).ToList();

            for (int m = appMenu.Count - 1; m >= 0; m--)
                appMenu[m].Items.RemoveAll(x => itemsToRemove.Contains(x.Class.ToUpper()));

            appMenu.RemoveAll(x => itemsToRemove.Contains(x.Class.ToUpper()));

            // Clear ResourceHash in class items
            appMenu.ForEach(m =>
            {
                m.Class = string.Empty;
                m.Items.ForEach(s => s.Class = string.Empty);
            });

            return appMenu;
        }

        [OperationRole(NotApply = true)]
        public ContentResult NotAllow(string routeDescription)
        {
            var cfg = new ContentUI
            {
                Header = new HtmlUIHeader()
                {
                    Title = $"Opção não permitida",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = "",
                SidebarUrl = @Url.Action("Sidebar")
            };

            cfg.Content.Add(new FormUI
            {
                Id = "fly01frm",
                Elements = new List<BaseUI>()
                {
                    new LabelSetUI()
                    {
                        Class = "col s12",
                        Id = "withoutpermission",
                        Name = "withoutpermission",
                        Label = $"Você não possui permissão no recurso {routeDescription}."
                    }
                },
                Class = "col s12"
            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        private string GenerateJWT()
        {
            var payload = new Dictionary<string, string>()
                {
                    {  "platformUrl", SessionManager.Current.UserData.PlatformUrl },
                    {  "email", SessionManager.Current.UserData.PlatformUser },
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
    }
}