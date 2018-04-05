using System;
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
        {
            return Request.IsAjaxRequest() ? Go() : base.Index();
        }

        public ContentResult Go()
        {
            return Content(JsonConvert.SerializeObject(HomeJson(true), JsonSerializerSetting.Front), "application/json");
        }

        public ContentResult List()
        {
            return Content(JsonConvert.SerializeObject(HomeJson(), JsonSerializerSetting.Front), "application/json");
        }
       
        public List<AppUI> AppsList()
        {
            List<AppUI> appsList = RestHelper.ExecuteGetRequest<List<AppUI>>(AppDefaults.UrlGateway.Replace("v2/compras", "v1"), String.Format("sidebarApps/{0}", SessionManager.Current.UserData.PlatformUrl), null);
            appsList.RemoveAll(x => x.Id == AppDefaults.AppIdCompras);
            return appsList;
        }

        protected abstract ContentUI HomeJson(bool withSidebarUrl = false);

        public abstract ContentResult Sidebar();
    }
}