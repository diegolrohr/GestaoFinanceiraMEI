using Fly01.Core.Config;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Fly01.Core.Presentation.Controllers
{
    public abstract class AccountController : Controller
    {
        [AllowAnonymous]
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

        [AllowAnonymous]
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