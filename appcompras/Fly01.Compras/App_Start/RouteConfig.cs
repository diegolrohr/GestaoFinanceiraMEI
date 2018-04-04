using System.Web.Mvc;
using System.Web.Routing;

namespace Fly01.Compras
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",

                // Qualquer alteração na DefaultRoute irá impactar nos JavaScripts
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
        }
    }
}
