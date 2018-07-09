using System.Web;
using System.Web.Mvc;

namespace Fly01.IntegracaoServiceBus.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
