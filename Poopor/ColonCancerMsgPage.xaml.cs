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
                StackPanel msgHolder = new StackPanel();
                msgHolder.Margin = new System.Windows.Thickness { Left = 15 };
                msgHolder.Orientation = System.Windows.Controls.Orientation.Horizontal;
                TextBlock dash = new TextBlock();
                dash.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                dash.FontFamily = new FontFamily("Segoe WP SemiLight");
                dash.FontSize = 24;
                dash.Text = "- ";
                TextBlock msg = new TextBlock();
                msg.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                msg.FontFamily = new FontFamily("Segoe WP SemiLight");
                msg.FontSize = 24;
                msg.Text = item;
                msgHolder.Children.Add(dash);
                msgHolder.Children.Add(msg);
                cancerSignMsg_area.Children.Add(msgHolder);
            }
        }
    }
}