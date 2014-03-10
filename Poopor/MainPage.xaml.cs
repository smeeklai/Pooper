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
using SQLite;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Poopor
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string userHealth;
        /*private Dictionary<string, object> userLastestResultsAndRecommendation = new Dictionary<string, object>()
        {
            {"usercancersign", "normal"},
        };*/

        public MainPage()
        {
            InitializeComponent();
            /*List<string> test = new List<string>();
            test.Add("test1");
            test.Add("test2");
            test.Add("- Your first-relative relationship members have been diagnosed with FAP or HNPCC");
            userLastestResultsAndRecommendation.Add("userCancerSignMsg", test);
            //var userLastestResultsAndRecommendation = SessionManagement.GetUserLastestResultsAndRecommendation();
            if (userLastestResultsAndRecommendation != null)
            {
                object userHealthObj = null;
                if (userLastestResultsAndRecommendation.TryGetValue("usercancersign", out userHealthObj))
                {
                    userHealth = userHealthObj as string;
                    if (!userHealth.Contains("normal"))
                        AdaptInterfaceToUserHealth(userHealth);
                }
                else
                    Debug.WriteLine("failed");
            }*/
            buildApplicationBar();
        }

        private void newPoop_button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PicturePage.xaml", UriKind.Relative));
        }

       /* private void AdaptInterfaceToUserHealth(string userHealth)
        {
            if (userHealth.Contains("suspicious"))
            {
                List<string> userCancerSignMsg = userLastestResultsAndRecommendation["userCancerSignMsg"] as List<string>;
                goodHealthInfo_grid.Visibility = System.Windows.Visibility.Collapsed;
                more_axiousSigns_text.Visibility = System.Windows.Visibility.Collapsed;
                suggestion_textBlock.Text = "Be careful with your health";
                representation_image.Source = new BitmapImage(new Uri("/Assets/img/risk/genrisk.png", UriKind.RelativeOrAbsolute));
                healthInfo_grid.Visibility = System.Windows.Visibility.Visible;
                SolidColorBrush newBgColor = new SolidColorBrush();
                newBgColor.Color = Color.FromArgb(255, 241, 145, 32);
                healthInfo_grid.Background = newBgColor;
                header_textBlock.Text = "Detected! general signs of colon-rectum cancer";
                try
                {
                    if (userCancerSignMsg[0] != null)
                        moreInfo_textBlock1.Text = userCancerSignMsg[0];
                    if (userCancerSignMsg[1] != null)
                        moreInfo_textBlock2.Text = userCancerSignMsg[1];
                    if (userCancerSignMsg[2] != null)
                        moreInfo_textBlock3.Text = userCancerSignMsg[2];
                }
                catch (ArgumentOutOfRangeException error)
                {

                }
            }
            else if (userHealth.Contains("dangerous"))
            {
                goodHealthInfo_grid.Visibility = System.Windows.Visibility.Collapsed;
                healthInfo_grid.Visibility = System.Windows.Visibility.Visible;
            }
        }*/

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
            if (userHealth.Contains("anxious"))
            {
                // User flicked towards right
                if (e.HorizontalVelocity > 0)
                {
                    NavigationService.Navigate(new Uri("/ResultPage.xaml", UriKind.Relative));
                }
            }
        }
    }
}