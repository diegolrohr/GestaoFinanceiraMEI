using System.Net.Http;

namespace Fly01.Core.Presentation
{
    public class HttpNotFoundAwareDefaultHttpControllerSelector : DefaultHttpControllerSelector
    {
        public HttpNotFoundAwareDefaultHttpControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor decriptor = null;

            try
            {
                decriptor = base.SelectController(request);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;
                //if (code != HttpStatusCode.NotFound)
                //    throw;

                var routeValues = request.GetRouteData().Values;
                if (routeValues.ContainsKey("action") && routeValues["action"].ToString() != "Handle404")
                    routeValues["oldAction"] = routeValues["action"];

                if (routeValues.ContainsKey("controller") && routeValues["controller"].ToString() != "ApiError")
                    routeValues["oldController"] = routeValues["controller"];

                routeValues["controller"] = "ApiError";
                routeValues["action"] = "Handle404";
                decriptor = base.SelectController(request);
            }
            return decriptor;
        }
    }
}