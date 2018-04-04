using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fly01.Compras.Models.SSO;
using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Helpers;
using Fly01.Compras.Models.ViewModel;

namespace Fly01.Compras.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string email = "")
        {
            if (Request.QueryString["t"] != null)
            {
                string t = Request.QueryString.Get("t");

                var claimsPrincipal = JWTHelper.Decode(t, "http://gestao.fly01.com.br/");

                var userName = JWTHelper.GetClaimValue(claimsPrincipal, "RazaoSocial");
                string platformUser = JWTHelper.GetClaimValue(claimsPrincipal, "Email");
                string fly01Url = JWTHelper.GetClaimValue(claimsPrincipal, "Fly01Url");

                return LoginGateway(fly01Url, platformUser, userName, returnUrl);
            }
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AppJWTAccessURL = string.Empty;
            if (Request.Url.Host.Contains("localhost"))
            {
                ViewBag.AppJWTAccessURL = Request.Url.AbsoluteUri;
                if (!string.IsNullOrWhiteSpace(Request.Url.Query))
                {
                    ViewBag.AppJWTAccessURL = Request.Url.AbsoluteUri.Replace(Request.Url.Query, "");
                }
            }
            var sAMLRequestAuthVM = new SAMLRequestAuthVM();
            return View(sAMLRequestAuthVM);
        }

        private ActionResult LoginGateway(string platformUrl, string platformUser, string userName, string returnUrl = "")
        {
            var tokenData = RestHelper.ExecuteGetAuthToken(AppDefaults.UrlGateway.Replace("v2/compras/", "v2/"), AppDefaults.GatewayUserName, AppDefaults.GatewayPassword, platformUrl, platformUser);
            var userData = new UserDataVM { TokenData = tokenData, PlatformUser = platformUser };

            userData.TokenData.Username = userName;
            userData.PlatformUrl = platformUrl;

            SessionManager.Current.UserData = userData;
            Response.SetAuthCookie(platformUser, true, new UserDataVM());

            string decodedUrl = "";

            if (!string.IsNullOrEmpty(returnUrl))
                decodedUrl = Server.UrlDecode(returnUrl);

            return RedirectToLocal(decodedUrl);
        }

        public ActionResult LogOff()
        {
            if (HttpContext.Session != null)
            {
                SystemLogOff(System.Web.HttpContext.Current);
            }

            return Redirect(AppDefaults.UrlLogoutSSO);
        }

        private ValidateVM ValidateToken(HttpContext httpContext)
        {
            ValidateVM validate = new ValidateVM
            {
                isValid = httpContext.User.Identity.IsAuthenticated && SessionManager.Current.UserData.IsValidUserData()
            };

            return validate;
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