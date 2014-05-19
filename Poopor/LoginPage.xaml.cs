using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SQLite;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Poopor.Resources;
using System.Windows.Controls.Primitives;
using System.IO.IsolatedStorage;
namespace Poopor
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private readonly DependencyProperty NetProperty = DependencyProperty.Register("NetworkAvailability",
                                         typeof(string),
                                         typeof(MainPage),
                                         new PropertyMetadata(string.Empty));

        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (SessionManagement.IsLoggedIn() == true)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void useAsGuest_button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void signIn_button_Click(object sender, RoutedEventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show(AppResources.NetworkUnavailable, AppResources.NoInternetConnection, MessageBoxButton.OK);
            }
            else
            {
                NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.Relative));
            }
        }

        private async void logIn_button_Click(object sender, RoutedEventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show(AppResources.NetworkUnavailable, AppResources.NoInternetConnection, MessageBoxButton.OK);
            }
            else if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (String.IsNullOrWhiteSpace(email_textBox.Text) || String.IsNullOrWhiteSpace(password_Box.Password) == true)
                {
                    MessageBox.Show("Email or Password cannot be empty", AppResources.Warning, MessageBoxButton.OK);
                }
                else
                {
                    SystemFunctions.SetProgressIndicatorProperties(true);
                    SystemTray.ProgressIndicator.Text = "authenticating...";
                    var succeeded = await new AzureFunctions().CheckUserAuthentication(email_textBox.Text, password_Box.Password);
                    if (succeeded)
                    {
                        var data = await new AzureFunctions().GetUserInfoDataAsync(email_textBox.Text);
                        try
                        {
                            string test = new SQLiteFunctions().GetUserInfo(email_textBox.Text).Username;
                        }
                        catch (NullReferenceException b)
                        {
                            Boolean resultOfInsertation = false;
                            while (resultOfInsertation == false)
                            {
                                resultOfInsertation = new SQLiteFunctions().InsertData(new UserInfo_Table_SQLite()
                                {
                                    Username = data.Email,
                                    Password = data.Password,
                                    FirstName = data.FirstName,
                                    LastName = data.LastName,
                                    DOB = data.DOB,
                                    Gender = data.Gender,
                                    Weight = data.Weight,
                                    Height = data.Height,
                                    HealthInfo1 = data.HealthInfo1,
                                    HealthInfo2 = data.HealthInfo2,
                                    HealthInfo3 = data.HealthInfo3,
                                    HealthInfo4 = data.HealthInfo4,
                                    HealthInfo5 = data.HealthInfo5,
                                });
                                Debug.WriteLine("Store old member into SQLite: " + resultOfInsertation);
                            }
                        }
                        SessionManagement.CreateLoginSession(email_textBox.Text);
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        MessageBox.Show(AppResources.IncorrectEmailOrPasswordMsg, AppResources.IncorrectEmailOrPasswordTitle, MessageBoxButton.OK);
                    }
                    SystemFunctions.SetProgressIndicatorProperties(false);
                }
            }
            else
            {
                SystemFunctions.ShowUnknownErrorMsgBox();
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
            Application.Current.Terminate();
        }
    }
}