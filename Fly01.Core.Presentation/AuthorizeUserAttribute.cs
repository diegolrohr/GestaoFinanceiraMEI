using Fly01.Core.ViewModels;
using System;

namespace Fly01.Core.Presentation
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthorizeUserAttribute : Attribute
    {
        public string ResourceKey { get; set; }
        public EPermissionValue PermissionValue { get; set; }
        //private string ControllerName { get; set; }
        //private string ActionName { get; set; }

        public AuthorizeUserAttribute(EPermissionValue permissionValue = EPermissionValue.Read)
        {
            PermissionValue = permissionValue;
        }

        public AuthorizeUserAttribute(string resourceKey, EPermissionValue permissionValue = EPermissionValue.Read)
            : this(permissionValue)
        {
            ResourceKey = resourceKey;
        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    //ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        //    //ActionName = filterContext.ActionDescriptor.ActionName;
        //    bool skipAuthorization = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
        //        || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();

        //    if (skipAuthorization || UserCanPerformOperation())
        //        return;

        //    HandleUnauthorizedRequest(filterContext);
        //}

        //private bool UserCanPerformOperation() 
        //    => SessionManager.Current.UserData.UserCanPerformOperation(ResourceKey, PermissionValue);
        
        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    if (filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        var routeValueDictionary = new RouteValueDictionary(new { controller = "Home", action = "Index" });
        //        filterContext.Result = new RedirectToRouteResult(routeValueDictionary);
        //    }
        //    else
        //        base.HandleUnauthorizedRequest(filterContext);
        //}
    }
}