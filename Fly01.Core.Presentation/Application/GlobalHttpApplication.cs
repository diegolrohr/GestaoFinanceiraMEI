using System;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Fly01.Core.Config;
using Fly01.Core.Rest;
using Microsoft.ApplicationInsights.Extensibility;

namespace Fly01.Core.Presentation.Application
{
    public class GlobalHttpApplication : HttpApplication
    {
        protected virtual string GetInstrumentationKeyAppInsights() => string.Empty;

        private static string ReadCookieAndSetSession(string token)
        {
            string platformUser = "";
            var formsAuthenticationTicket = FormsAuthentication.Decrypt(token);

            if ((formsAuthenticationTicket != null) && (formsAuthenticationTicket.UserData != null))
            {
                dynamic cookieUserData = Json.Decode(formsAuthenticationTicket.UserData);

                platformUser = cookieUserData.UserName;
                string platformUrl = cookieUserData.Fly01Url;

                TokenDataVM tokenData = RestHelper.ExecuteGetAuthToken(
                    AppDefaults.UrlGateway, AppDefaults.GatewayUserName,
                    AppDefaults.GatewayPassword, platformUrl, platformUser);

                var userData = new UserDataVM
                {
                    TokenData = tokenData,
                    PlatformUser = platformUser,
                    PlatformUrl = platformUrl
                };

                userData.TokenData.Username = cookieUserData.Name;
                SessionManager.Current.UserData = userData;
            }
            return platformUser;
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if ((Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState) &&
                SessionManager.Current.UserData.IsValidUserData() &&
                ((HttpContext.Current.User == null) || (HttpContext.Current.User.Identity.IsAuthenticated == false)))
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.RemoveAll();
                FormsAuthentication.SignOut();

                if (Request.Headers["X-Requested-With"] != null &&
                    Request.Headers["X-Requested-With"].ToUpper().Equals("XMLHTTPREQUEST"))
                    FormsAuthentication.RedirectToLoginPage();
                else
                {
                    Response.Write(
                        String.Format("<script type=\"text/javascript\">top.location.href='{0}';</script>",
                            FormsAuthentication.LoginUrl));
                    Response.End();
                }
            }
            else
            {
                if (FormsAuthentication.CookiesSupported && Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    HttpContext.Current.User =
                        new GenericPrincipal(
                            new GenericIdentity(
                                ReadCookieAndSetSession(Request.Cookies[FormsAuthentication.FormsCookieName].Value),
                                "Forms"),
                            string.Empty.Split(';'));
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
            AppDefaults.GatewayUserName = ConfigurationManager.AppSettings["GatewayUserName"];
            AppDefaults.GatewayPassword = ConfigurationManager.AppSettings["GatewayPassword"];
            AppDefaults.GatewayVerificationKeyPassword = ConfigurationManager.AppSettings["GatewayVerificationKeyPassword"];
            AppDefaults.UrlLoginSSO = ConfigurationManager.AppSettings["UrlLoginSSO"];
            AppDefaults.UrlLogoutSSO = ConfigurationManager.AppSettings["UrlLogoutSSO"];
            AppDefaults.UrlLicenseManager = ConfigurationManager.AppSettings[""];
            AppDefaults.SessionKey = ConfigurationManager.AppSettings["SessionKey"];
            AppDefaults.AppId = ConfigurationManager.AppSettings["AppId"];
            AppDefaults.RootPathApplication = ConfigurationManager.AppSettings["RootPathApplication"];
            AppDefaults.APIEnumResourceName = "Fly01.Core.Entities.Domains.Enum.";

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
                HttpContext.Current.User =
                    new GenericPrincipal(
                        new GenericIdentity(
                            ReadCookieAndSetSession(Request.Cookies[FormsAuthentication.FormsCookieName].Value), "Forms"),
                        string.Empty.Split(';'));
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
                    Response.Write(String.Format("<script type=\"text/javascript\">top.location.href='{0}?t={1}';</script>", FormsAuthentication.LoginUrl, Request.QueryString["t"]));
                    Response.End();
                }
                else
                {
                    Response.Write(String.Format("<script type=\"text/javascript\">top.location.href='{0}';</script>", FormsAuthentication.LoginUrl));
                    Response.End();
                }
            }
        }

        protected void Session_End(object sender, EventArgs e) => FormsAuthentication.SignOut();
    }
}
