using System.Web.Http.Filters;

namespace Fly01.Core.Api
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public AllowCrossSiteJsonAttribute(string allowDomain = "*")
        {
            AllowDomain = allowDomain;
        }
        private string AllowDomain { get; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response?.Headers.Add("Access-Control-Allow-Origin", AllowDomain);

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}