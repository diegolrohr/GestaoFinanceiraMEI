using System;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Fly01.Core;
using System.Configuration;
using Fly01.Estoque.Controllers;
using Fly01.Core.Config;

namespace Fly01.Estoque
{
    public class WebApiApplication : HttpApplication
    {
        protected void Session_Start(Object sender, EventArgs e)
        {
            string cookieHeaders = HttpContext.Current.Request.Headers["Cookie"];

            if (((cookieHeaders == null) || (cookieHeaders.IndexOf("ASP.NET_SessionId", StringComparison.Ordinal) < 0)) ||
                (!Request.Url.LocalPath.Equals(FormsAuthentication.LoginUrl, StringComparison.InvariantCultureIgnoreCase)))
            {
                AccountController.SystemLogOff(HttpContext.Current);

                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"].ToUpper().Equals("XMLHTTPREQUEST"))
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

        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            if ((Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState) &&
                SessionManager.Current.UserData.IsValidUserData() &&
                ((HttpContext.Current.User == null) || (HttpContext.Current.User.Identity.IsAuthenticated == false)))
            {

                AccountController.SystemLogOff(HttpContext.Current);
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"].ToUpper().Equals("XMLHTTPREQUEST"))
                    FormsAuthentication.RedirectToLoginPage();
                else
                {
                    Response.Write(
                        String.Format("<script type=\"text/javascript\">top.location.href='{0}';</script>",
                            FormsAuthentication.LoginUrl));
                    Response.End();
                }
            }
        }

        private void SetAppDefaults()
        {
            AppDefaults.MashupClientId = ConfigurationManager.AppSettings["MashupClientId"];
            AppDefaults.MashupPassword = ConfigurationManager.AppSettings["MashupPassword"];
            AppDefaults.MashupUser = ConfigurationManager.AppSettings["MashupUser"];
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlS1Gateway"];
            AppDefaults.GatewayUserName = ConfigurationManager.AppSettings["GatewayUserName"];
            AppDefaults.GatewayPassword = ConfigurationManager.AppSettings["GatewayPassword"];
            AppDefaults.GatewayVerificationKeyPassword = ConfigurationManager.AppSettings["GatewayVerificationKeyPassword"];
            AppDefaults.UrlLoginSSO = ConfigurationManager.AppSettings["UrlLoginSSO"];
            AppDefaults.UrlLogoutSSO = ConfigurationManager.AppSettings["UrlLogoutSSO"];
            AppDefaults.UrlLicenseManager = ConfigurationManager.AppSettings[""];
            AppDefaults.SessionKey = ConfigurationManager.AppSettings["SessionKey"];
            AppDefaults.AppIdFinanceiro = ConfigurationManager.AppSettings["AppIdFinanceiro"];
            AppDefaults.AppIdFaturamento = ConfigurationManager.AppSettings["AppIdFaturamento"];
            AppDefaults.AppIdCompras = ConfigurationManager.AppSettings["AppIdCompras"];
            AppDefaults.AppIdEstoque = ConfigurationManager.AppSettings["AppIdEstoque"];
            AppDefaults.AppIdSaude = ConfigurationManager.AppSettings["AppIdSaude"];
            AppDefaults.RootPathApplication = ConfigurationManager.AppSettings["RootPathApplication"];
        }

        protected void Application_Start()
        {
            SetAppDefaults();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BundleTable.EnableOptimizations = true;

            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
        }

        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        var formsAuthenticationTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                        if (formsAuthenticationTicket != null)
                        {
                            string username = formsAuthenticationTicket.Name;
                            string roles = string.Empty;

                            e.User = new GenericPrincipal(
                                new GenericIdentity(username, "Forms"), roles.Split(';'));
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported && Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                try
                {
                    var formsAuthenticationTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                    if (formsAuthenticationTicket == null) return;
                    string username = formsAuthenticationTicket.Name;
                    string roles = string.Empty;

                    HttpContext.Current.User = new GenericPrincipal(
                        new GenericIdentity(username, "Forms"), roles.Split(';'));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
