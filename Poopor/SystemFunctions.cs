using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Poopor.Resources;

namespace Poopor
{
    class SystemFunctions
    {
        public SystemFunctions() { 
            
        }

        public static Boolean IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                    @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

        public static Boolean IsConfirmPasswordMatched(string confirmPassword, string password)
        {
            if (confirmPassword.Length == password.Length && confirmPassword.Contains(password))
                return true;
            return false;
        }

        public static void SetProgressIndicatorProperties(bool isVisible)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        public static void ShowUnknownErrorMsgBox()
        {
            MessageBox.Show(AppResources.UnknownErrorMsg, AppResources.Warning, MessageBoxButton.OK);
        }
    }
}
