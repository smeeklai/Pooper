using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    class SessionManagement
    {
        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        private static readonly String LOGIN_STATUS = "loginStatus";
        private static readonly String EMAIL = "email";

        public static Boolean IsLoggedIn()
        {
            if (settings.Contains(LOGIN_STATUS))
                return (Boolean)settings[LOGIN_STATUS];
            else
                return false;
        }

        public static void CreateLoginSession(String userEmail)
        {
            settings.Add(EMAIL, userEmail);
            settings.Add(LOGIN_STATUS, true);
            settings.Save();
        }

        public static String GetEmail()
        {
            if (settings.Contains(EMAIL))
                return settings[EMAIL] as String;
            else
                return null;
        }

        public static async Task<bool> Logout()
        {
            if (settings.Contains(LOGIN_STATUS))
                settings.Remove(LOGIN_STATUS);
            if (settings.Contains(EMAIL))
                settings.Remove(EMAIL);
            return true;
        }
    }
}
