using System.Web;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Config;
using System.Web.Security;
using Newtonsoft.Json;
using Fly01.Core.Defaults;
using Fly01.uiJS.Responses;
using System.Linq;

namespace Fly01.Core.Presentation.Controllers
{
    [AllowAnonymous]
    public abstract class AccountController : Controller
    {

        public ContentResult Platforms()
        {
            LoginResponse response = RestHelper.ExecutePostRequest<LoginResponse>(
                AppDefaults.UrlGateway, 
                "v1/Platforms", 
                JsonConvert.SerializeObject(new { platformUser = SessionManager.Current.UserData.PlatformUser })
            );

            return Content(JsonConvert.SerializeObject(response, JsonSerializerSetting.Front), "application/json");
        }

        public ActionResult Login(string returnUrl, string email = "")
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            ViewBag.LoginUrl = AppDefaults.UrlLoginSSO;
            return View();
        }

        public ActionResult LogOff()
        {
            if (HttpContext.Session != null)
                SystemLogOff(System.Web.HttpContext.Current);
            return Redirect(AppDefaults.UrlLogoutSSO);
        }

        private bool ValidateToken(HttpContext httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated && SessionManager.Current.UserData.IsValidUserData();
        }

        public JsonResult ValidateTokenJson()
        {
            return Json(ValidateToken(System.Web.HttpContext.Current), JsonRequestBehavior.AllowGet);
        }

        public static void SystemLogOff(HttpContext context)
        {
            context.Session.Clear();
            context.Session.Abandon();
            context.Session.RemoveAll();
            FormsAuthentication.SignOut();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) ? (ActionResult)Redirect(returnUrl) : RedirectToAction("Index", "Home");
        }
    }
}