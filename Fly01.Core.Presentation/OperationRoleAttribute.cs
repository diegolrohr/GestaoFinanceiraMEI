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
    }
}