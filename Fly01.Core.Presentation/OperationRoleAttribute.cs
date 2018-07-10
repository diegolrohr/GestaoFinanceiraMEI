using Fly01.Core.ViewModels;
using System;

namespace Fly01.Core.Presentation
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class OperationRoleAttribute : Attribute
    {
        public string ResourceKey { get; set; }
        public EPermissionValue PermissionValue { get; set; }
        public bool NotApply { get; set; }

        //public OperationRoleAttribute(string resourceKey, EPermissionValue permissionValue = EPermissionValue.Read)
        //    : this(permissionValue)
        //{
        //    ResourceKey = resourceKey;
        //}

        //public OperationRoleAttribute(EPermissionValue permissionValue = EPermissionValue.Read)
        //{
        //    PermissionValue = permissionValue;
        //}
    }
}