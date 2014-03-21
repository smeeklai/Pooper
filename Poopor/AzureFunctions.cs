﻿using Microsoft.WindowsAzure.MobileServices;
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
            
        }

        public async Task<Boolean> InsertDataAsync(object data)
        {
            try
            {
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
                    SessionManagement.RememberUserLatestPoopTime(((Poop_Table_Azure)data).Date_Time);
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

        public async void UpdateData(object data)
        {

        }

        public async void DeleteData(string index)
        {

        }

        public async Task<bool> CheckUserAuthentication(String userEmail, String userPassword)
        {
            try
            {
                items = await azure_userInfo_table.ToCollectionAsync();
                Debug.WriteLine("Retrieve data successfully");
                foreach (var item in items)
                {
                    if (item.Email.Equals(userEmail) && item.Password.Equals(userPassword))
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
            Debug.WriteLine("email and password are mismatched");
            return false;
        }

        public async Task<MobileServiceCollection<Poop_Table_Azure, Poop_Table_Azure>> GetUserPoopDataInAzure(string userEmail)
        {
            try
            {
                var items = await azure_poop_table.Where(Poop_Table_Azure => Poop_Table_Azure.Email == userEmail).ToCollectionAsync();
                return items;
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<MobileServiceCollection<Poop_Table_Azure, Poop_Table_Azure>> GetUserPoopDataAfterInputDate(string userEmail ,DateTime dateTime)
        {
            try
            {
                var items = await azure_poop_table.Where(Poop_Table_Azure => Poop_Table_Azure.Email == userEmail).Where(Poop_Table_Azure => Poop_Table_Azure.Date_Time > dateTime).ToCollectionAsync();
                return items;
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
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

        public bool InsertData(object data)
        {
            return true;
        }

        public async Task<UserInfo_Table_Azure> GetUserInfoDataAsync(string userEmail)
        {
            try
            {
                var items = await azure_userInfo_table.Where(UserInfo_Table_Azure => UserInfo_Table_Azure.Email == userEmail).ToCollectionAsync();
                var result = items.FirstOrDefault();
                return result;
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
