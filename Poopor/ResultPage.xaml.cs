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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Poopor
{
    public partial class ResultPage : PhoneApplicationPage
    {
        private Dictionary<string, object> userLastestResultAndRecommendation;
        private SolidColorBrush newBgColor = new SolidColorBrush();
        private SolidColorBrush newBgColor2 = new SolidColorBrush();

        // Constructor
        public ResultPage()
        {
            InitializeComponent();
            userLastestResultAndRecommendation = SessionManagement.GetUserLastestResultsAndRecommendation();

            var userLastestPoopData = new SQLiteFunctions().GetUserPoopData(SessionManagement.GetEmail()).Last();
            colorRecord_textBlock.Text = userLastestPoopData.Color;
            shapeRecord_textBlock.Text = userLastestPoopData.Shape;
            painLevelRecord_textBlock.Text = userLastestPoopData.Pain_Level;
            bloodAmountRecord_textBlock.Text = userLastestPoopData.Blood_Amount;

            if (GetUserCancerSign().Equals("general"))
            {
                cancer_area.Tap += cancer_area_Tap;
                newBgColor.Color = Color.FromArgb(255, 242, 175, 96);
                newBgColor2.Color = Color.FromArgb(255, 241, 145, 32);
                resultArea.Background = newBgColor;
                cancer_area.Background = newBgColor2;
                resultHeader_textBlock.Text = "TAP HERE!";
                resultImage.Source = new BitmapImage(new Uri("/Assets/img/risk/genrisk.png", UriKind.RelativeOrAbsolute));
                resultExplaination_textBlock.Text = "General signs of colon-rectum cancer have been detected";
            }
            else if (GetUserCancerSign().Equals("anxious"))
            {
                cancer_area.Tap += cancer_area_Tap;
                newBgColor.Color = Color.FromArgb(255, 238, 118, 118);
                newBgColor2.Color = Color.FromArgb(255, 230, 74, 74);
                resultArea.Background = newBgColor;
                cancer_area.Background = newBgColor2;
                resultHeader_textBlock.Text = "TAP HERE!";
                resultImage.Source = new BitmapImage(new Uri("/Assets/img/risk/anxiousrisk.png", UriKind.RelativeOrAbsolute));
                resultExplaination_textBlock.Text = "Anxious signs of colon-rectum cancer have been detected";
            }
            if (!GetRecommendationStatus())
            {
                recommendation_textBlock.Visibility = Visibility.Collapsed;
                recommendationResult_area.Visibility = Visibility.Collapsed;
                noRecommendationResult_area.Visibility = Visibility.Visible;
                newBgColor.Color = Color.FromArgb(255, 130, 241, 196);
                recommendations_header.Visibility = Visibility.Collapsed;
                recommendation_lists.Visibility = Visibility.Collapsed;
                recommendations_area.Background = newBgColor;
            }
            else
            {
                List<string> userShortRecommendation = userLastestResultAndRecommendation["UserShortRecommendation"] as List<string>;

            }
        }

        private Boolean GetRecommendationStatus()
        {
            
            object recommendationStatus = null;
            if (userLastestResultAndRecommendation.TryGetValue("recommendationStatus", out recommendationStatus))
                return (Boolean)recommendationStatus;
            else
                return false;
        }

        private string GetUserCancerSign()
        {
            object userCancerSign = null;
            if (userLastestResultAndRecommendation.TryGetValue("UserCancerSign", out userCancerSign))
                return userCancerSign as string;
            else
                return null;
        }

        private void cancer_area_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        } 
    }
}