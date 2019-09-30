using Fly01.Core.Config;
using Fly01.Core.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fly01.Core.Presentation.Controllers
{
    [AllowAnonymous]
    public abstract class PrimitiveBaseController : Controller
    {
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = true;
                //filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                //filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (skipAuthorization)
                return;
            else
                HandleUnauthorizedRequest(filterContext);
        }

        protected void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var isJson = (Request.Headers["Accept"] != null && Request.Headers["Accept"].Contains("application/json"));
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "NotAllow"
                    })
                );
            }
        }
    }
}