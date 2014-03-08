using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Poopor.Resources;
using System.Diagnostics;

namespace Poopor
{
    public partial class MainPage : PhoneApplicationPage
    {

        public MainPage()
        {
            InitializeComponent();
            buildApplicationBar();
        }

        private void newPoop_button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PicturePage.xaml", UriKind.Relative));
        }

        private void buildApplicationBar()
        {
            //Initialize ApplicationBar
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 0.8;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            //Initialize ApplicationBarMenuItem
            ApplicationBarMenuItem logoutAppBar = new ApplicationBarMenuItem();
            logoutAppBar.Text = AppResources.AppBarLogout;
            logoutAppBar.Click += new EventHandler(logoutAppBar_Click);

            //Add menu item
            ApplicationBar.MenuItems.Add(logoutAppBar);
        }

        private async void logoutAppBar_Click(object sender, EventArgs e)
        {
            await SessionManagement.Logout();
            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void OnFlick(object sender, FlickGestureEventArgs e)
        {
            // User flicked towards right
            if (e.HorizontalVelocity > 0)
            {
                NavigationService.Navigate(new Uri("/ResultPage.xaml", UriKind.Relative));
            }
        }
    }
}