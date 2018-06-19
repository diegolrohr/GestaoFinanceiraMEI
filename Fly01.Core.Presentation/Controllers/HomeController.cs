using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Core.Config;
using Fly01.Core.Rest;

namespace Fly01.Core.Presentation.Controllers
{
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
    }
}