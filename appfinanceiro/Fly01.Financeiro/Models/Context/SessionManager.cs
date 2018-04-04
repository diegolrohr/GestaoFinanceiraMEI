using System;
using System.Web;

namespace Fly01.Financeiro.Models.Context
{
    [Serializable]
    public class SessionManager
    {
        private const string SessionKey = "_SESSION_MANAGER_FLY01_APP_S1_FINANCEIRO_";
        //public static string sessionKey;

        public SessionManager()
        {
            userData = new UserDataVM();
        }

        public static SessionManager Current
        {
            get 
            { 
                if (HttpContext.Current.Session[SessionKey] == null)
                {
                    HttpContext.Current.Session[SessionKey] = new SessionManager();
                }

                return (SessionManager)HttpContext.Current.Session[SessionKey];
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
                    HttpContext.Current.Session[SessionKey] = null;
            }
        }
    }

    public static class UserDataVMExtension
    {
        public static bool IsValidUserData(this UserDataVM userDataVM)
        {
            return (
                (userDataVM.TokenData != null) &&
                (!string.IsNullOrWhiteSpace(userDataVM.TokenData.AccessToken))
            );
        }
    }
}