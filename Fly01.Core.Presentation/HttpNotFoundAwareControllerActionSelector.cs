namespace Fly01.Core.Presentation
{
    public class HttpNotFoundAwareControllerActionSelector : ApiControllerActionSelector
    {
        public HttpNotFoundAwareControllerActionSelector()
        {
        }

        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            HttpActionDescriptor decriptor = null;
            try
            {
                decriptor = base.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;
                //if (code != HttpStatusCode.NotFound && code != HttpStatusCode.MethodNotAllowed)
                //    throw;

                var routeData = controllerContext.RouteData;

                if (routeData.Values.ContainsKey("action") && routeData.Values["action"].ToString() != "Handle404")
                    routeData.Values.Add("oldAction", routeData.Values["action"]);
                if (routeData.Values.ContainsKey("controller") && routeData.Values["controller"].ToString() != "ApiError")
                    routeData.Values.Add("oldController", routeData.Values["controller"]);

                routeData.Values["action"] = "Handle404";
            }
            return decriptor;
        }
    }
}