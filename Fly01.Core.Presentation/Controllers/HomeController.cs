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
    [AllowAnonymous]
    public abstract class HomeController : GenericAppController
    {
        public override ActionResult Index()
            => Request.IsAjaxRequest() ? Go() : base.Index();

        public ContentResult Go()
            => Content(JsonConvert.SerializeObject(HomeJson(), JsonSerializerSetting.Front), "application/json");

        public ContentResult List()
            => Content(JsonConvert.SerializeObject(HomeJson(), JsonSerializerSetting.Front), "application/json");

        protected abstract ContentUI HomeJson();

        public abstract ContentResult Sidebar();
    }
}