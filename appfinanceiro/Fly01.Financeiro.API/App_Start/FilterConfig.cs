using System.Web.Mvc;

namespace Fly01.Financeiro.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new SessionAuthorizeAttribute());
        }
    }
}
