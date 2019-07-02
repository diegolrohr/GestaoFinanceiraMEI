using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Fly01.Core.Config;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Web.SessionState;

namespace Fly01.Core.Presentation.Application
{
    public static class HttpResponseBaseExtensions
    {
        public static int SetAuthCookie<T>(this HttpResponseBase responseBase, string name, bool rememberMe, T userData)
        {
            var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, JsonConvert.SerializeObject(userData), ticket.CookiePath);
            var encTicket = FormsAuthentication.Encrypt(newTicket);

            cookie.Domain = AppDefaults.UrlGateway.Contains("fly01local")
                ? ".bemacashlocal.com.br"
                : AppDefaults.UrlGateway.Contains("fly01dev")
                    ? ".bemacashstage.com.br"
                    : ".bemacash.com.br";

            cookie.Value = encTicket;
            responseBase.Cookies.Add(cookie);

            return encTicket.Length;
        }
    }

    public class GlobalHttpApplication : HttpApplication
    {
        protected virtual string GetInstrumentationKeyAppInsights() => string.Empty;

        private static List<PermissionResponseVM> GetPermissionsByUser(string platformUrl, string platformUser)
        {
            try
            {
                var requestObject = JsonConvert.SerializeObject(new { platformUrl, platformUser/*, resouceRoot = AppDefaults.AppId*/ });
                var response = RestHelper.ExecutePostRequest<List<PermissionResponseVM>>(AppDefaults.UrlGateway, "v2/Permission", requestObject);
                return response ?? new List<PermissionResponseVM>();
            }
            catch
            {
                return new List<PermissionResponseVM>();
            }
        }

        private static bool ReadCookieAndSetSession(string token)
        {
            var formsAuthenticationTicket = FormsAuthentication.Decrypt(token);
            if ((formsAuthenticationTicket != null) && (formsAuthenticationTicket.UserData != null))
            {
                UserDataCookieVM cookieUserData = Json.Decode<UserDataCookieVM>(formsAuthenticationTicket.UserData);

                UserDataVM userData = new UserDataVM()
                {
                    PlatformUser = cookieUserData.UserName,
                    PlatformUrl = cookieUserData.Fly01Url
                };
                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(cookieUserData.UserName, "Forms"), string.Empty.Split(';'));
                if (!SessionManager.Current.UserData.IsValidUserData(userData))
                {
                    try
                    {
                        TokenDataVM tokenData = RestHelper.ExecuteGetAuthToken(
                            AppDefaults.UrlGateway, AppDefaults.GatewayUserName,
                            AppDefaults.GatewayPassword, cookieUserData.Fly01Url, cookieUserData.UserName);
                        userData.TokenData = tokenData;
                        userData.TokenData.UserName = cookieUserData.Name;
                        userData.Permissions = GetPermissionsByUser(cookieUserData.Fly01Url, cookieUserData.UserName);

                        SessionManager.Current.UserData = userData;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if ((Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState) &&
                ((HttpContext.Current.User == null) || (HttpContext.Current.User.Identity.IsAuthenticated == false)))
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.RemoveAll();
                FormsAuthentication.SignOut();

                if (Request.Headers["Accept"] != null && Request.Headers["Accept"].Contains("application/json"))
                {
                    Response.Write(JsonConvert.SerializeObject(new { urlToRedirect = AppDefaults.UrlLoginSSO }));
                    Response.End();
                }
                else if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"].ToUpper().Equals("XMLHTTPREQUEST"))
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                else
                {
                    Response.Write($"<script type=\"text/javascript\">top.location.href='{AppDefaults.UrlLoginSSO}';</script>");
                    Response.End();
                }
            }
            else if (FormsAuthentication.CookiesSupported && Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                if (!ReadCookieAndSetSession(Request.Cookies[FormsAuthentication.FormsCookieName].Value))
                {
                    Response.Write($"<script type=\"text/javascript\">top.location.href='{AppDefaults.UrlManager}';</script>");
                    Response.End();
                }
            }
        }

        protected void Application_Start()
        {
            string instrumentationKeyAppInsights = GetInstrumentationKeyAppInsights();

            if (!string.IsNullOrWhiteSpace(instrumentationKeyAppInsights))
                TelemetryConfiguration.Active.InstrumentationKey = instrumentationKeyAppInsights;

            AppDefaults.MashupClientId = ConfigurationManager.AppSettings["MashupClientId"];
            AppDefaults.MashupPassword = ConfigurationManager.AppSettings["MashupPassword"];
            AppDefaults.MashupUser = ConfigurationManager.AppSettings["MashupUser"];
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlGateway"];
            AppDefaults.UrlApiGateway = String.Format("{0}{1}", AppDefaults.UrlGateway, ConfigurationManager.AppSettings["GatewayAppApi"]);
            AppDefaults.UrlGatewayNew = $"{ConfigurationManager.AppSettings["UrlGatewayNew"]}api/";
            AppDefaults.UrlManagerNew = $"{AppDefaults.UrlGatewayNew}manager/";
            AppDefaults.UrlManager = ConfigurationManager.AppSettings["UrlManager"];
            AppDefaults.AppId = ConfigurationManager.AppSettings["AppId"];
            AppDefaults.UrlLoginSSO = String.Format(ConfigurationManager.AppSettings["UrlLoginSSO"], AppDefaults.UrlManager, AppDefaults.AppId);
            AppDefaults.UrlLogoutSSO = String.Format(ConfigurationManager.AppSettings["UrlLogoutSSO"], AppDefaults.UrlManager, AppDefaults.AppId);
            AppDefaults.GatewayUserName = ConfigurationManager.AppSettings["GatewayUserName"];
            AppDefaults.GatewayPassword = ConfigurationManager.AppSettings["GatewayPassword"];
            AppDefaults.GatewayVerificationKeyPassword = ConfigurationManager.AppSettings["GatewayVerificationKeyPassword"];
            AppDefaults.UrlLicenseManager = ConfigurationManager.AppSettings[""];
            AppDefaults.SessionKey = ConfigurationManager.AppSettings["SessionKey"];
            AppDefaults.RootPathApplication = ConfigurationManager.AppSettings["RootPathApplication"];
            AppDefaults.APIEnumResourceName = "Fly01.Core.Entities.Domains.Enum.";
            AppDefaults.FaturamentoClientId = ConfigurationManager.AppSettings["FaturamentoClientId"];
            AppDefaults.UrlNotificationSocket = ConfigurationManager.AppSettings["UrlNotificationSocket"];

            GlobalConfiguration.Configure(WebAPIConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if ((Request.Cookies["ASP.NET_SessionId"] != null) &&
                (Request.Url.LocalPath.Equals(FormsAuthentication.LoginUrl,
                    StringComparison.InvariantCultureIgnoreCase)))
                return;
            if (FormsAuthentication.CookiesSupported &&
                Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                if (!ReadCookieAndSetSession(Request.Cookies[FormsAuthentication.FormsCookieName].Value))
                {
                    //HttpCookie mpnData = new HttpCookie("mpndata") { Expires = DateTime.UtcNow.AddDays(2), Path = "/" };
                    //mpnData.Values["UserEmail"] = SessionManager.Current.UserData.PlatformUser;
                    //mpnData.Values["UserName"] = SessionManager.Current.UserData.TokenData.UserName;
                    //mpnData.Values["TrialUntil"] = SessionManager.Current.UserData.TokenData.Trial
                    //    ? SessionManager.Current.UserData.TokenData.LicenseExpirationString : "";
                    //Response.Cookies.Add(mpnData);
                }
            }
            else
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.RemoveAll();
                FormsAuthentication.SignOut();

                if (Request.Headers["X-Requested-With"] != null &&
                        Request.Headers["X-Requested-With"].ToUpper().Equals("XMLHTTPREQUEST"))
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                else if (Request.AppRelativeCurrentExecutionFilePath.Equals("~/") && Request.QueryString["t"] != null)
                {
                    Response.Write($"<script type=\"text/javascript\">top.location.href='{AppDefaults.UrlLoginSSO}?t={Request.QueryString["t"]}';</script>");
                    Response.End();
                }
                else
                {
                    Response.Write($"<script type=\"text/javascript\">top.location.href='{AppDefaults.UrlLoginSSO}';</script>");
                    Response.End();
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
            => FormsAuthentication.SignOut();
    }
}
