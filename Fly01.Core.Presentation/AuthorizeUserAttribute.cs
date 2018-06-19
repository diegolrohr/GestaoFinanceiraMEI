using Fly01.Core.Config;
using Fly01.Core.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fly01.Core.Presentation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public string ResourceKey { get; set; }
        public EPermissionValue PermissionValue { get; set; }
        //private string ControllerName { get; set; }
        //private string ActionName { get; set; }

        public AuthorizeUserAttribute(string resourceKey, EPermissionValue permissionValue = EPermissionValue.Read)
        {
            ResourceKey = resourceKey;
            PermissionValue = permissionValue;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //ActionName = filterContext.ActionDescriptor.ActionName;

            bool skipAuthorization = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();

            if (skipAuthorization || UserCanPerformOperation())
                return;

            HandleUnauthorizedRequest(filterContext);
        }

        private bool UserCanPerformOperation() 
            => SessionManager.Current.UserData.UserCanPerformOperation(ResourceKey, PermissionValue);

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var routeValueDictionary = new RouteValueDictionary(new { controller = "Home", action = "Index" });
                filterContext.Result = new RedirectToRouteResult(routeValueDictionary);
            }
            else
                base.HandleUnauthorizedRequest(filterContext);
        }
    }
}