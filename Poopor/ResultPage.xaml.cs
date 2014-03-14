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
using System.Diagnostics;

namespace Poopor
{
    public partial class ResultPage : PhoneApplicationPage
    {
        
        // Constructor
        public ResultPage()
        {
            InitializeComponent();
            //userLastestResultAndRecommendation = SessionManagement.GetUserLastestResultsAndRecommendation();
            //AdjustResultArea();
            //AdjustRecommendationArea();
            //AdjustMeaningArea();
        }

        private void AdjustResultArea()
        {
            var userLastestPoopData = new SQLiteFunctions().GetUserPoopData("test").Last();
            colorRecord_textBlock.Text = userLastestPoopData.Color;
            shapeRecord_textBlock.Text = userLastestPoopData.Shape;
            painLevelRecord_textBlock.Text = userLastestPoopData.Pain_Level;
            bloodAmountRecord_textBlock.Text = userLastestPoopData.Blood_Amount;
            dateTimeRecord_textBlock.Text = userLastestPoopData.Date_Time.ToShortDateString();
            

            /*if (GetUserCancerSign().Equals("general"))
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
            }*/
        }

        private void AdjustRecommendationArea()
        {
            if (!GetRecommendationStatus())
            {
                TextBlock noRecommendation_textBlock = new TextBlock();
                noRecommendation_textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60));
                noRecommendation_textBlock.FontFamily = new FontFamily("Segoe WP SemiLight");
                noRecommendation_textBlock.Text = "No recommendation! You are healthy";
                recommendation_lists.Children.Add(noRecommendation_textBlock);
            }
            else
            {
                List<string> userShortRecommendation = userLastestResultAndRecommendation["UserShortRecommendation"] as List<string>;
                try
                {
                    if (userShortRecommendation[0] != null)
                    {
                        recommendation_image1.Source = shortRecommendation_dictionary[userShortRecommendation[0]];
                        recommendation_text1.Text = userShortRecommendation[0];
                    }
                    if (userShortRecommendation[1] != null)
                    {
                        recommendation_image2.Source = shortRecommendation_dictionary[userShortRecommendation[1]];
                        recommendation_text2.Text = userShortRecommendation[1];
                    }
                    if (userShortRecommendation[2] != null)
                    {
                        recommendation_image3.Source = shortRecommendation_dictionary[userShortRecommendation[2]];
                        recommendation_text3.Text = userShortRecommendation[2];
                    }
                    if (userShortRecommendation[3] != null)
                    {
                        recommendation_image4.Source = shortRecommendation_dictionary[userShortRecommendation[3]];
                        recommendation_text4.Text = userShortRecommendation[3];
                    }
                    if (userShortRecommendation[4] != null)
                    {
                        recommendation_image5.Source = shortRecommendation_dictionary[userShortRecommendation[4]];
                        recommendation_text5.Text = userShortRecommendation[4];
                    }
                    if (userShortRecommendation[5] != null)
                    {
                        recommendation_image6.Source = shortRecommendation_dictionary[userShortRecommendation[5]];
                        recommendation_text6.Text = userShortRecommendation[5];
                    }
                }
                catch (ArgumentOutOfRangeException error)
                {

                }
                List<string> longRecommendation = userLastestResultAndRecommendation["UserLongRecommendation"] as List<string>;
                foreach (string item in longRecommendation)
                {
                    TextBlock newLongRecommendation_textBlock = new TextBlock();
                    newLongRecommendation_textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60));
                    newLongRecommendation_textBlock.FontFamily = new FontFamily("Segoe WP SemiLight");
                    newLongRecommendation_textBlock.Text = "- " + item;
                    recommendation_lists.Children.Add(newLongRecommendation_textBlock);
                }
            }
        }

        private void AdjustMeaningArea()
        {
            List<string> poopColorMeaning = userLastestResultAndRecommendation["UserPoopColorMeaning"] as List<string>;
            foreach (string item in poopColorMeaning)
            {
                TextBlock newColorMeaning_textBlock = new TextBlock();
                newColorMeaning_textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60));
                newColorMeaning_textBlock.FontFamily = new FontFamily("Segoe WP SemiLight");
                newColorMeaning_textBlock.Text = "- " + item;
                poopColorMeaning_lists.Children.Add(newColorMeaning_textBlock);
            }
            List<string> poopShapeMeaning = userLastestResultAndRecommendation["UserPoopShapeMeaning"] as List<string>;
            foreach (string item in poopShapeMeaning)
            {
                TextBlock newPoopMeaning_textBlock = new TextBlock();
                newPoopMeaning_textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60));
                newPoopMeaning_textBlock.FontFamily = new FontFamily("Segoe WP SemiLight");
                newPoopMeaning_textBlock.Text = "- " + item;
                poopShapeMeaning_lists.Children.Add(newPoopMeaning_textBlock);
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

        private Dictionary<string, object> userLastestResultAndRecommendation;
        private SolidColorBrush newBgColor = new SolidColorBrush();
        private SolidColorBrush newBgColor2 = new SolidColorBrush();
        private Dictionary<string, BitmapImage> shortRecommendation_dictionary = new Dictionary<string, BitmapImage>()
        {
            {"Fruits and vegetables", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Fiber", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Fat diet", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Alcohol", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Iron", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Consult a doctor", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Gastric irritation", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Water or fluids", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Supplements", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Herbal teas", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Cooked grains", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Meat", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Dairy", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Wheat", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Eggs", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Refined Carbo", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Refined sugar", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"De-stress", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))},
            {"Food allergies", new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute))}
        };
    }
}