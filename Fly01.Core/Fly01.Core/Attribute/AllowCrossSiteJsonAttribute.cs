namespace Fly01.Core.Attribute
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