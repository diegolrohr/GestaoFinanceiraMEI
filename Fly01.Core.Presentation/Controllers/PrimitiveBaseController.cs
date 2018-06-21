using Fly01.Core.Config;
using Fly01.Core.ViewModels;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fly01.Core.Presentation.Controllers
{
    public abstract class PrimitiveBaseController : Controller
    {
        public string ResourceHashPermissao { get; set; }

        private bool UserCanPerformOperation(string resourceKey, EPermissionValue permissionValue)
            => SessionManager.Current.UserData.UserCanPerformOperation(resourceKey, permissionValue);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            bool skipAuthorization =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            var resourceKey = "";
            var permissionValue = EPermissionValue.Read;

            if (skipAuthorization || UserCanPerformOperation(resourceKey, permissionValue))
                return;
            else
                HandleUnauthorizedRequest(filterContext);
        }

        protected void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // auth failed, redirect to login page
            //filterContext.Result = new HttpUnauthorizedResult();

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var routeValueDictionary = new RouteValueDictionary(new { controller = "Home", action = "Index" });
                filterContext.Result = new RedirectToRouteResult(routeValueDictionary);
            }
            else
            {
                //https://stackoverflow.com/questions/977071/redirecting-unauthorized-controller-in-asp-net-mvc

                ViewDataDictionary viewData = new ViewDataDictionary
                {
                    { "Message", "You do not have sufficient privileges for this operation." }
                };
                filterContext.Result = new ViewResult { MasterName = "Home", ViewName = "Index", ViewData = viewData };

            }
        }
    }
}