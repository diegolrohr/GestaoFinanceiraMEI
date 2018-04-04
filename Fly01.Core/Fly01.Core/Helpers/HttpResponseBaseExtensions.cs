using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace Fly01.Core.Helpers
{
    public static class HttpResponseBaseExtensions
    {
        public static int SetAuthCookie<T>(this HttpResponseBase responseBase, string name, bool rememberMe, T userData)
        {            
            var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);

            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            string userDataTicket = JsonConvert.SerializeObject(userData);

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, userDataTicket, ticket.CookiePath);
            var encTicket = FormsAuthentication.Encrypt(newTicket);

            //if (encTicket.Length > AppDefaults.CookieMaxByteSize)
            //    throw new ApplicationException(String.Format("Não foi possível autenticar na aplicação. O ticket de autenticação possui {0} bytes. (Limite máximo: {1})", encTicket.Length, AppDefaults.CookieMaxByteSize));

            cookie.Value = encTicket;

            responseBase.Cookies.Add(cookie);

            return encTicket.Length;
        }
    }
}