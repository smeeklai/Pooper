using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static readonly String IMAGE_SAVED_COUNTER = "imageSavedCounter";

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

        public static int GetImageSavedCounter()
        {
            int result;
            if (settings.Contains(IMAGE_SAVED_COUNTER))
            {
                result = (int)settings[IMAGE_SAVED_COUNTER];
                result++;
                settings[IMAGE_SAVED_COUNTER] = result;
            }
            else
            {
                result = 1;
                settings.Add(IMAGE_SAVED_COUNTER, result);
            }
            settings.Save();
            return result;
        }

        public static void RemoveImage()
        {
            if (settings.Contains(IMAGE_SAVED_COUNTER))
            {
                settings.Remove(IMAGE_SAVED_COUNTER);
                Debug.WriteLine("Table deleted");
            }
            else
            {
                Debug.WriteLine("Table doesn't exist");
            }
            settings.Save();
        }

        public static async Task<bool> Logout()
        {
            if (settings.Contains(LOGIN_STATUS))
                settings.Remove(LOGIN_STATUS);
            if (settings.Contains(EMAIL))
                settings.Remove(EMAIL);
            settings.Save();
            return true;
        }
    }
}
