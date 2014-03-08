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
            MessageBoxResult result = MessageBox.Show("You're using as a guest. The result will not be recorded and reliable because of insufficient personal information and historical data",
                "Warning", MessageBoxButton.OK);

            if (result == MessageBoxResult.OK)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }

        }

        private void signIn_button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.Relative));
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
                    MessageBox.Show("Email and Password cannot be empty", AppResources.Warning, MessageBoxButton.OK);
                }
                else
                {
                    var succeeded = await new AzureFunctions().CheckUserAuthentication(email_textBox.Text, password_Box.Password);
                    if (succeeded)
                    {
                        SessionManagement.CreateLoginSession(email_textBox.Text);
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        MessageBox.Show(AppResources.IncorrectEmailOrPasswordMsg, AppResources.IncorrectEmailOrPasswordTitle, MessageBoxButton.OK);
                    }
                }
            }
            else
            {
                SystemFunctions.ShowUnknownErrorMsgBox();
            }
        }
    }
}