using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poopor.Resources;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;

namespace Poopor
{
    class AzureFunctions : DatabaseFunctions
    {
        private IMobileServiceTable<UserInfo_Table_Azure> azure_userInfo_table = App.MobileService.GetTable<UserInfo_Table_Azure>();
        private IMobileServiceTable<Poop_Table_Azure> azure_poop_table = App.MobileService.GetTable<Poop_Table_Azure>();
        private static Poop_Table_Azure poop_table = new Poop_Table_Azure();
        private static UserInfo_Table_Azure userInfo_table = new UserInfo_Table_Azure();
        private MobileServiceCollection<UserInfo_Table_Azure, UserInfo_Table_Azure> items;

        public AzureFunctions()
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
        }

        public async Task<Boolean> InsertData(object data)
        {
            try
            {
                SystemFunctions.SetProgressIndicatorProperties(true);
                SystemTray.ProgressIndicator.Text = "Registering...";

                //Check type of data
                if (IsUserInfo_Data(data))
                {
                    //Insert data into UserInfo_Table
                    await azure_userInfo_table.InsertAsync((UserInfo_Table_Azure)data);
                    Debug.WriteLine("Success inserting data to azure");
                    SystemFunctions.SetProgressIndicatorProperties(false);
                }
                else if (IsPoop_Data(data))
                {
                    //Insert data into Poop_Table
                    await azure_poop_table.InsertAsync((Poop_Table_Azure)data);
                    Debug.WriteLine("Success");
                }
                return true;
            }
            catch (MobileServiceInvalidOperationException e)
            {
                SystemFunctions.SetProgressIndicatorProperties(false);
                Debug.WriteLine("Failed: " + e.Message);
            }
            return false;
        }

        public async void getData()
        {
            try
            {
                items = await azure_userInfo_table.ToCollectionAsync();
                Debug.WriteLine(items.Count);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async void UpdateData(object data)
        {

        }

        public async void DeleteData(string index)
        {

        }

        public async Task<bool> CheckUserAuthentication(String userEmail, String userPassword)
        {
            SystemFunctions.SetProgressIndicatorProperties(true);
            SystemTray.ProgressIndicator.Text = "authenticating...";
            try
            {
                items = await azure_userInfo_table.ToCollectionAsync();
                Debug.WriteLine(items.Count);
                Debug.WriteLine("Retrieve data successfully");
                foreach (var item in items)
                {
                    if (item.Email.Contains(userEmail) && item.Password.Contains(userPassword))
                    {
                        SystemFunctions.SetProgressIndicatorProperties(false);
                        Debug.WriteLine("email and password are matched");
                        return true;
                    }
                }
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }
            SystemFunctions.SetProgressIndicatorProperties(false);
            Debug.WriteLine("email and password are mismatched");
            return false;
        }


        private Boolean IsUserInfo_Data(object data)
        {
            if (userInfo_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }

        private Boolean IsPoop_Data(object data)
        {
            if (poop_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }
    }
}
