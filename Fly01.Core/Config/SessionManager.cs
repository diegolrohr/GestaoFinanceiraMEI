using System;
using System.Web;

namespace Fly01.Core.Config
{
    [Serializable]
    public class SessionManager
    {
        //private const string SessionKey = "_SESSION_MANAGER_FLY01_APP_S1_FINANCEIRO_";
        //public static string sessionKey;

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
                {
                    HttpContext.Current.Session[AppDefaults.SessionKey] = new SessionManager();
                }

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
    }
}