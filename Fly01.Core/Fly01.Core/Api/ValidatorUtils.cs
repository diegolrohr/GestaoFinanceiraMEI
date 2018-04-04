using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;

namespace Fly01.Core.Api
{
    public static class ValidatorUtils
    {
        public static object GetDynamicMember(object obj, string memberName)
        {
            var binder = Binder.GetMember(CSharpBinderFlags.None, memberName, obj.GetType(),
                new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
            var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
            return callsite.Target(callsite, obj);
        }

        public static bool ExistDynamicMember(object obj, string memberName)
        {
            try
            {
                GetDynamicMember(obj, memberName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}