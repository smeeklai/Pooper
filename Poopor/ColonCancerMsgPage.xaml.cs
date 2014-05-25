using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Poopor.Data;
using System.Windows.Media;

namespace Poopor
{
    public partial class ColonCancerMsgPage : PhoneApplicationPage
    {
        public ColonCancerMsgPage()
        {
            InitializeComponent();
            ListUserCancerSignMsg(SessionManagement.GetUserLastestResultsAndRecommendation()["UserCancerSignMsg"]);
        }

        private void ListUserCancerSignMsg(List<string> listMsg){
            foreach (string item in SystemFunctions.SortByLength(listMsg))
            {
                TextBlock msg = new TextBlock();
                msg.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                msg.FontFamily = new FontFamily("Segoe WP SemiLight");
                msg.FontSize = 24;
                msg.TextWrapping = TextWrapping.Wrap;
                msg.Text = "- " + item;
                //msgHolder.Children.Add(dash);
                //msgHolder.Children.Add(msg);
                cancerSignMsg_area.Children.Add(msg);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!SessionManagement.IsLoggedIn())
            {
                SessionManagement.Logout();
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}