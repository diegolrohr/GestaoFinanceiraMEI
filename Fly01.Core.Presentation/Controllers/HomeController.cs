using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Core.Config;
using Fly01.Core.Rest;
using Fly01.uiJS.Classes.Elements;
using System.Linq;

namespace Fly01.Core.Presentation.Controllers
{
    [OperationRole(NotApply = true)]
    public abstract class HomeController : GenericAppController
    {
        public override ActionResult Index() 
            => Request.IsAjaxRequest() ? Go() : base.Index();

        public ContentResult Go() 
            => Content(JsonConvert.SerializeObject(HomeJson(true), JsonSerializerSetting.Front), "application/json");

        public ContentResult List() 
            => Content(JsonConvert.SerializeObject(HomeJson(), JsonSerializerSetting.Front), "application/json");

        public List<AppUI> AppsList()
        {
            var requestObject = new { platformUrl = SessionManager.Current.UserData.PlatformUrl, platformUser = SessionManager.Current.UserData.PlatformUser, originApp = AppDefaults.AppId };
            return RestHelper.ExecutePostRequest<List<AppUI>>(AppDefaults.UrlGateway, "v1/sidebarApps", requestObject);
        }

        protected abstract ContentUI HomeJson(bool withSidebarUrl = false);

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

            // Clear resourceHash in class items
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
                UrlFunctions = ""
            };

            cfg.Content.Add(new FormUI()
            {
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
    }
}