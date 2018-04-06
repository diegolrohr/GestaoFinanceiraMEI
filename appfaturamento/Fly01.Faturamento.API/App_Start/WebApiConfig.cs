using Fly01.Core.API;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Fly01.Faturamento.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApiWithAction", "api/{controller}/{action}");
            config.Routes.MapHttpRoute("DefaultApiGet", "api/{controller}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute("DefaultApiPost", "api/{controller}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Convention-based routing (Hello Page).
            config.Routes.MapHttpRoute(
                name: "DefaultHelloPage",
                routeTemplate: "{controller}/{action}",
                defaults: new { controller = "Hello", action = "Index" }
            );

            config.Routes.MapHttpRoute(
                name: "Error404",
                routeTemplate: "{*url}",
                defaults: new { controller = "ApiError", action = "Handle404" }
            );

            config.Filters.Add(new CustomExceptionFilter());
        }
    }
}
