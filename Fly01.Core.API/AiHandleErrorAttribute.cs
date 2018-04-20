using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace Fly01.Core.API
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    //public class AiHandleErrorAttribute : HandleErrorAttribute, IFilter
    //{
    //    public override void OnException(ExceptionContext filterContext)
    //    {
    //        if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
    //        {
    //            if (filterContext.HttpContext.IsCustomErrorEnabled)
    //            {
    //                var ai = new TelemetryClient();
    //                ai.TrackException(filterContext.Exception);
    //            }
    //        }
    //        base.OnException(filterContext);
    //    }
    //}

    public class AiHandleErrorAttribute : ExceptionLogger, IFilter
    {
        public bool AllowMultiple { get { return true; } }

        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                // Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
                var ai = new TelemetryClient();
                ai.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}