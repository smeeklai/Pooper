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

namespace Poopor
{
    public partial class ResultPage : PhoneApplicationPage
    {
        // Constructor
        public ResultPage()
        {
            InitializeComponent();
            var test = new SQLiteFunctions().GetUserPoopData(SessionManagement.GetEmail());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void resultArea_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //Navigate to risk signs explaination page
        }


        // public static void StoreUserLastestResultsAndRecommendation(Dictionary<string, object> lastestResultAndRecommendation)
        //public static Dictionary<string, object> GetUserLastestResultsAndRecommendation()
        
    }
}