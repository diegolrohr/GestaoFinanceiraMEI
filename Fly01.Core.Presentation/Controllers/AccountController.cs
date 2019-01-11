using System;
using System.Web;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Config;
using Fly01.Core.Helpers;
using System.Web.Security;
using System.Web.Configuration;
using System.Collections.Generic;
using Fly01.Core.Presentation.SSO;
using Newtonsoft.Json;

namespace Fly01.Core.Presentation.Controllers
{
    [AllowAnonymous]
    public abstract class AccountController : Controller
    {
        public ActionResult Login(string returnUrl, string email = "")
        {
            return View("LoginSSO", new SSOGatewayRequest()
            {
                AssertionUrl = Url.Action("Assertion", "Account", null, Request.Url.Scheme),
                AppId = WebConfigurationManager.AppSettings["GatewayUserName"],
                AppPassword = WebConfigurationManager.AppSettings["GatewayPassword"],
                SSOUrl = AppDefaults.UrlLoginSSO
            });
        }

        [HttpPost]
        public ActionResult Assertion()
        {
            var assertion = new AssertionResponseVM(Request.Form);

            var dataPermission = RestUtils.ExecuteGetRequest<ResponseDataVM<DataUserPermissionVM>>(AppDefaults.UrlManager, "user/permissions", RestUtils.GetAuthHeader(assertion.TokenType + " " + assertion.AccessToken), new Dictionary<string, string>()
            {
                { "platformUrl", assertion.PlatformUrl },
                { "email", assertion.UserEmail },
            });

            var tokenDataVM = new TokenDataVM
            {
                AccessToken = assertion.AccessToken,
                ExpiresIn = assertion.ExpiresIn,
                TokenType = assertion.TokenType,
                Username = assertion.Username
            };

            UserDataVM userDataVM = new UserDataVM
            {
                PlatformUser = assertion.UserEmail,
                PlatformUrl = assertion.PlatformUrl,
                TokenData = tokenDataVM,
                Permissions = dataPermission.Data.Items,
                ClientToken = assertion.ClientToken
            };

            SessionManager.Current.UserData = userDataVM;

            HttpCookie mpnData = new HttpCookie("mpndata") { Expires = DateTime.UtcNow.AddDays(2), Path = "/" };
            mpnData.Values["UserName"] = SessionManager.Current.UserData.PlatformUser;
            mpnData.Values["UserEmail"] = assertion.UserEmail;
            mpnData.Values["TrialUntil"] = assertion.Trial ? assertion.LicenseExpiration : "";

            Response.Cookies.Add(mpnData);

            FormsAuthentication.SetAuthCookie(SessionManager.Current.UserData.PlatformUser, false);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            var clientToken = SessionManager.Current.UserData.ClientToken;

            if (HttpContext.Session != null)
                SystemLogOff(System.Web.HttpContext.Current);

            return Redirect($"{AppDefaults.UrlLogoutSSO}/{clientToken}");
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