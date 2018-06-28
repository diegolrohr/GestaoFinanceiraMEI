using Fly01.Core.Config;
using Fly01.Core.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fly01.Core.Presentation.Controllers
{
    public abstract class PrimitiveBaseController : Controller
    {
        private bool UserCanPerformOperation(string resourceKey, EPermissionValue permissionValue)
            => SessionManager.Current.UserData.UserCanPerformOperation(resourceKey, permissionValue);

        private AuthorizeUserAttribute GetOperationRoles(ActionDescriptor action)
        {
            var findAnnotationInherit = true;

            var annotationInAction = action.GetCustomAttributes(findAnnotationInherit)
                .OfType<AuthorizeUserAttribute>()
                .FirstOrDefault();

            var annotationInController = action.ControllerDescriptor.GetCustomAttributes(findAnnotationInherit)
                .OfType<AuthorizeUserAttribute>()
                .FirstOrDefault();

            //var resourceKey = string.Empty;
            //var permissionValue = EPermissionValue.Read;
            
            if (annotationInAction == null && annotationInController == null)
                throw new Exception("annotationInAction == null and annotationInController == null");
            //else
            //{

            //}
            
            return annotationInAction ?? annotationInController;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = filterContext.ActionDescriptor.ActionName;

            //base.OnAuthorization(filterContext);

            bool skipAuthorization =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            var operationRoles = GetOperationRoles(filterContext.ActionDescriptor);

            if (skipAuthorization || UserCanPerformOperation(operationRoles.ResourceKey, operationRoles.PermissionValue))
                return;
            else
                HandleUnauthorizedRequest(filterContext);
        }

        //protected override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    bool skipAuthorization =
        //        filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
        //        filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

        //    if (!skipAuthorization)
        //    {
        //        base.OnAuthorization(filterContext);
        //    }
        //}

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