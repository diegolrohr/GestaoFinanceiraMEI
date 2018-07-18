using Fly01.Core.ViewModels;
using System;
using System.Linq;
using System.Web;

namespace Fly01.Core.Config
{
    [Serializable]
    public class SessionManager
    {
        public SessionManager()
        {
            userData = new UserDataVM();
        }

        public static SessionManager Current
        {
            get
            {
                if (HttpContext.Current.Session == null)
                    return new SessionManager();

                if (HttpContext.Current.Session[AppDefaults.SessionKey] == null)
                    HttpContext.Current.Session[AppDefaults.SessionKey] = new SessionManager();

                return (SessionManager)HttpContext.Current.Session[AppDefaults.SessionKey];
            }
        }

        private UserDataVM userData;
        public UserDataVM UserData
        {
            get { return userData; }
            set 
            {
                userData = value;
                if(value == null)
                    HttpContext.Current.Session[AppDefaults.SessionKey] = null;
            }
        }
    }

    public static class UserDataVMExtension
    {
        public static bool IsValidUserData(this UserDataVM userDataVM)
        {
            return (
                (userDataVM != null) &&
                (userDataVM.TokenData != null) &&
                (userDataVM.PlatformUrl != null) &&
                (userDataVM.PlatformUser != null) &&
                (!string.IsNullOrWhiteSpace(userDataVM.TokenData.AccessToken))
            );
        }
        public static bool IsValidUserData(this UserDataVM userDataVM, UserDataVM testUserData)
        {
            return (
                (userDataVM != null) && (testUserData != null) &&
                (userDataVM.PlatformUrl != null) && (userDataVM.PlatformUrl == testUserData.PlatformUrl) &&
                (userDataVM.PlatformUser != null) && (userDataVM.PlatformUser == testUserData.PlatformUser)
            );
        }

        public static bool UserCanPerformOperation(this UserDataVM userDataVM, string resourceHash, EPermissionValue operation = EPermissionValue.Read)
        {
            if (!IsValidUserData(userDataVM))
                return false;
            
            var permission = userDataVM.Permissions.FirstOrDefault(x => x.ResourceHash.Equals(resourceHash, StringComparison.InvariantCultureIgnoreCase));
            var canOperation = permission != null && (int)permission.PermissionValue >= (int)operation;

            return canOperation;
        }
    }
}