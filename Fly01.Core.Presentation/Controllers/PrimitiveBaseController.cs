using Fly01.Core.Config;
using Fly01.Core.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fly01.Core.Presentation.Controllers
{
    public abstract class PrimitiveBaseController : Controller
    {
        protected string ResourceHash { get; set; }
        protected bool UserCanRead { get; set; }
        protected bool UserCanWrite { get; set; }

        private const bool FindAnnotationInherit = true;

        private string ControllerName { get; set; }
        private string ActionName { get; set; }
        
        public PrimitiveBaseController()
        {
            var annotationInController = GetType().GetCustomAttributes(FindAnnotationInherit)
               .OfType<OperationRoleAttribute>()
               .FirstOrDefault();

            ResourceHash = string.Empty;
            if (annotationInController != null)
            {
                ResourceHash = annotationInController.ResourceKey ?? string.Empty;

                UserCanRead = annotationInController.NotApply || UserCanPerformOperation(ResourceHash, EPermissionValue.Read);
                UserCanWrite = annotationInController.NotApply || UserCanPerformOperation(ResourceHash, EPermissionValue.Write);
            }
        }

        public bool UserCanPerformOperation(string resourceKey, EPermissionValue permissionValue = EPermissionValue.Read)
            => SessionManager.Current.UserData.UserCanPerformOperation(resourceKey, permissionValue);

        private OperationRoleAttribute GetOperationRole(ActionDescriptor action)
        {
            var annotationInAction = action.GetCustomAttributes(FindAnnotationInherit)
                .OfType<OperationRoleAttribute>()
                .FirstOrDefault();

            var annotationInController = action.ControllerDescriptor.GetCustomAttributes(FindAnnotationInherit)
                .OfType<OperationRoleAttribute>()
                .FirstOrDefault();

            var resourceKey = string.Empty;
            var permissionValue = EPermissionValue.Read;
            var notApply = false;

            if (annotationInAction != null || annotationInController != null)
            {
                if (annotationInController != null)
                {
                    resourceKey = annotationInController.ResourceKey;
                    permissionValue = annotationInController.PermissionValue;
                    notApply = annotationInController.NotApply;
                }
                //else : Sem else para garantir que o 'annotationInAction' sobreescreva o comportamento do controller 
                if (annotationInAction != null)
                {
                    resourceKey = !string.IsNullOrEmpty(annotationInAction.ResourceKey) ? annotationInAction.ResourceKey : resourceKey;
                    permissionValue = annotationInAction.PermissionValue;
                    notApply = !notApply ? annotationInAction.NotApply : notApply;
                }
            }
            else
                notApply = true;
            //throw new Exception("'annotationInAction' == null and 'annotationInController' == null in 'PrimitiveBaseController'");

            if (string.IsNullOrEmpty(resourceKey) && !notApply)
                notApply = true;
            //throw new Exception("Invalid 'resourceKey' in 'PrimitiveBaseController'");

            return new OperationRoleAttribute() { ResourceKey = resourceKey, PermissionValue = permissionValue, NotApply = notApply };
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            ActionName = filterContext.ActionDescriptor.ActionName;

            bool skipAuthorization =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            var operationRoles = GetOperationRole(filterContext.ActionDescriptor);

            if (skipAuthorization || operationRoles.NotApply || UserCanPerformOperation(operationRoles.ResourceKey, operationRoles.PermissionValue))
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
                        controller = isJson ? "Home" : ControllerName,
                        action = "NotAllow",
                        routeDescription = $"{ControllerName}/{ActionName}"
                    })
                );
            }
        }
    }
}