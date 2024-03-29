﻿using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Fly01.Core.API.Application
{
    public static class WebAPIConfig
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

            config.Routes.MapHttpRoute(
                name: "DefaultHelloPage",
                routeTemplate: "{controller}/{action}",
                defaults: new { controller = "Hello", action = "Say" }
            );

            config.Routes.MapHttpRoute(
                name: "Error404",
                routeTemplate: "{*url}",
                defaults: new { controller = "ApiError", action = "Handle404" }
            );
            
            config.Filters.Add(new CustomExceptionFilter());
            //config.Filters.Add(new AiHandleErrorAttribute());

            //Avaliar
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = 
                new DefaultContractResolver { IgnoreSerializableAttribute = true };
        }
    }
}
