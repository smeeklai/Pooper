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
using Poopor.Data;
using System.IO.IsolatedStorage;

namespace Poopor
{
    public partial class ResultPage : PhoneApplicationPage
    {
        private Poop_Table_SQLite userLastestPoopData;
        private string poopColor;
        private string poopShape;
        private string bloodAmount;
        private string painLevel;
        private string userPoopDateTime;

        // Constructor
        public ResultPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (SessionManagement.IsLoggedIn())
            {
                userLastestPoopData = new SQLiteFunctions().GetUserPoopData(SessionManagement.GetEmail()).Last();
                poopColor = userLastestPoopData.Color;
                poopShape = userLastestPoopData.Shape;
                painLevel = userLastestPoopData.Pain_Level;
                bloodAmount = userLastestPoopData.Blood_Amount;
                userPoopDateTime = userLastestPoopData.Date_Time.ToString();
            }
            else
            {
                NavigationContext.QueryString.TryGetValue("poopColor", out poopColor);
                NavigationContext.QueryString.TryGetValue("shape", out poopShape);
                NavigationContext.QueryString.TryGetValue("painLevel", out bloodAmount);
                NavigationContext.QueryString.TryGetValue("bloodAmount", out painLevel);
                NavigationContext.QueryString.TryGetValue("userPoopStoredDateTime", out userPoopDateTime);
            }

            colorRecord_textBlock.Text = poopColor;
            shapeRecord_textBlock.Text = poopShape;
            painLevelRecord_textBlock.Text = painLevel;
            bloodAmountRecord_textBlock.Text = bloodAmount;
            dateTimeRecord_textBlock.Text = userPoopDateTime;

            userLastestResultAndRecommendation = SessionManagement.GetUserLastestResultsAndRecommendation();
            if (userLastestResultAndRecommendation != null)
            {
                AdjustResultArea();
                AdjustRecommendationArea();
                AdjustMeaningArea();
            }
        }

        private void AdjustResultArea()
        {
            if (GetUserCancerSign().Equals("general"))
            {
                cancer_area.Tap += cancer_area_Tap;
                newBgColor.Color = Color.FromArgb(255, 242, 175, 96);
                newBgColor2.Color = Color.FromArgb(255, 241, 145, 32);
                resultArea.Background = newBgColor;
                cancer_area.Background = newBgColor2;
                resultHeader_textBlock.Text = "WARNING!";
                resultImage.Source = new BitmapImage(new Uri("/Assets/img/risk/genrisk.png", UriKind.RelativeOrAbsolute));
                resultExplaination_textBlock.Text = "Marginal risks of colon-rectum cancer have been detected";
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
                resultExplaination_textBlock.Text = "Critical risks of colon-rectum cancer have been detected";
            }
        }

        private void AdjustRecommendationArea()
        {
            if (!IsRecommendationExisted())
            {
                recommendation_textBlock.Visibility = System.Windows.Visibility.Collapsed;
                recommendationResult_area.Visibility = System.Windows.Visibility.Collapsed;
                noRecommendationResult_area.Visibility = System.Windows.Visibility.Visible;
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
                        Debug.WriteLine(userShortRecommendation[0]);
                        recommendation_image1.Source = shortRecommendation_dictionary[userShortRecommendation[0]];
                        recommendation_text1.Text = userShortRecommendation[0];
                    }
                    if (userShortRecommendation[1] != null)
                    {
                        Debug.WriteLine(userShortRecommendation[1]);
                        recommendation_image2.Source = shortRecommendation_dictionary[userShortRecommendation[1]];
                        recommendation_text2.Text = userShortRecommendation[1];
                    }
                    if (userShortRecommendation[2] != null)
                    {
                        Debug.WriteLine(userShortRecommendation[2]);
                        recommendation_image3.Source = shortRecommendation_dictionary[userShortRecommendation[2]];
                        recommendation_text3.Text = userShortRecommendation[2];
                    }
                    if (userShortRecommendation[3] != null)
                    {
                        Debug.WriteLine(userShortRecommendation[3]);
                        recommendation_image4.Source = shortRecommendation_dictionary[userShortRecommendation[3]];
                        recommendation_text4.Text = userShortRecommendation[3];
                    }
                    if (userShortRecommendation[4] != null)
                    {
                        Debug.WriteLine(userShortRecommendation[4]);
                        recommendation_image5.Source = shortRecommendation_dictionary[userShortRecommendation[4]];
                        recommendation_text5.Text = userShortRecommendation[4];
                    }
                    if (userShortRecommendation[5] != null)
                    {
                        Debug.WriteLine(userShortRecommendation[5]);
                        recommendation_image6.Source = shortRecommendation_dictionary[userShortRecommendation[5]];
                        recommendation_text6.Text = userShortRecommendation[5];
                    }
                }
                catch (ArgumentOutOfRangeException error)
                {

                }
                List<string> longRecommendation = userLastestResultAndRecommendation["UserLongRecommendation"] as List<string>;
                foreach (string item in SystemFunctions.SortByLength(longRecommendation))
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
            poopColorMeaning_arc.Fill = AccentColorNameToBrush.ConvertStringToSolidColorBrush(poopColor);
            colorResult_textBlock.Text = poopColor;

            shapeMeaning_image.Source = ShapeTypeToImg.ConvertShapeStringToImg(poopShape);
            shapeResult_textBlock.Text = poopShape;

            List<string> poopColorMeaning = userLastestResultAndRecommendation["UserPoopColorMeaning"] as List<string>;
            foreach (string item in SystemFunctions.SortByLength(poopColorMeaning))
            {
                TextBlock newColorMeaning_textBlock = new TextBlock();
                newColorMeaning_textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60));
                newColorMeaning_textBlock.FontFamily = new FontFamily("Segoe WP SemiLight");
                newColorMeaning_textBlock.Text = "- " + item;
                poopColorMeaning_lists.Children.Add(newColorMeaning_textBlock);
            }
            List<string> poopShapeMeaning = userLastestResultAndRecommendation["UserPoopShapeMeaning"] as List<string>;
            foreach (string item in SystemFunctions.SortByLength(poopShapeMeaning))
            {
                TextBlock newPoopMeaning_textBlock = new TextBlock();
                newPoopMeaning_textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60));
                newPoopMeaning_textBlock.FontFamily = new FontFamily("Segoe WP SemiLight");
                newPoopMeaning_textBlock.Text = "- " + item;
                poopShapeMeaning_lists.Children.Add(newPoopMeaning_textBlock);
            }
        }

        private Boolean IsRecommendationExisted()
        {
            List<string> recommendationStatus = null;
            if (userLastestResultAndRecommendation.TryGetValue("NecessaryInfo", out recommendationStatus))
                return Convert.ToBoolean(recommendationStatus[2]);
            else
                return false;
        }

        private string GetUserCancerSign()
        {
            List<string> userCancerSign = null;
            if (userLastestResultAndRecommendation.TryGetValue("NecessaryInfo", out userCancerSign))
                return userCancerSign[1];
            else
                return null;
        }

        private void cancer_area_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (GetUserCancerSign().Equals("anxious"))
            {
                NavigationService.Navigate(new Uri("/ColonCancerMsgPage.xaml", UriKind.Relative));
            }
        }


        private void okayResult_button_Click(object sender, RoutedEventArgs e)
        {
            if (!SessionManagement.IsLoggedIn())
            {
                SessionManagement.Logout();
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (SessionManagement.IsLoggedIn())
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            else
            {
                SessionManagement.Logout();
            }
            Application.Current.Terminate();
        }

        private Dictionary<string, List<string>> userLastestResultAndRecommendation;
        private SolidColorBrush newBgColor = new SolidColorBrush();
        private SolidColorBrush newBgColor2 = new SolidColorBrush();
        private Dictionary<string, BitmapImage> shortRecommendation_dictionary = new Dictionary<string, BitmapImage>()
        {
            {"Alcohol", new BitmapImage(new Uri("/Assets/img/recIcons/Alcohol.png", UriKind.RelativeOrAbsolute))},
            {"BRAT diet", new BitmapImage(new Uri("/Assets/img/recIcons/BRAT diet.png", UriKind.RelativeOrAbsolute))},
            {"Consult a doctor", new BitmapImage(new Uri("/Assets/img/recIcons/Consult a doctor.png", UriKind.RelativeOrAbsolute))},
            {"Cooked grains", new BitmapImage(new Uri("/Assets/img/recIcons/Cooked grains.png", UriKind.RelativeOrAbsolute))},
            {"De-stress", new BitmapImage(new Uri("/Assets/img/recIcons/De-stress.png", UriKind.RelativeOrAbsolute))},
            {"Fat diet", new BitmapImage(new Uri("/Assets/img/recIcons/Fat diet.png", UriKind.RelativeOrAbsolute))},
            {"Fiber", new BitmapImage(new Uri("/Assets/img/recIcons/Fiber.png", UriKind.RelativeOrAbsolute))},
            {"Food allergies", new BitmapImage(new Uri("/Assets/img/recIcons/Food allergies.png", UriKind.RelativeOrAbsolute))},
            {"Fruits and vegetables", new BitmapImage(new Uri("/Assets/img/recIcons/Fruits and vegetables.png", UriKind.RelativeOrAbsolute))},
            {"Gastric irritation", new BitmapImage(new Uri("/Assets/img/recIcons/Gastric irritation.png", UriKind.RelativeOrAbsolute))},
            {"Herbal teas", new BitmapImage(new Uri("/Assets/img/recIcons/Herbal teas.png", UriKind.RelativeOrAbsolute))},
            {"Iron", new BitmapImage(new Uri("/Assets/img/recIcons/Iron.png", UriKind.RelativeOrAbsolute))},
            {"Meat", new BitmapImage(new Uri("/Assets/img/recIcons/Meat.png", UriKind.RelativeOrAbsolute))},
            {"Refined Carbo", new BitmapImage(new Uri("/Assets/img/recIcons/Refined Carbo.png", UriKind.RelativeOrAbsolute))},
            {"Refined sugar", new BitmapImage(new Uri("/Assets/img/recIcons/Refined sugar.png", UriKind.RelativeOrAbsolute))},
            {"Supplements", new BitmapImage(new Uri("/Assets/img/recIcons/Supplements.png", UriKind.RelativeOrAbsolute))},
            {"Water or fluids", new BitmapImage(new Uri("/Assets/img/recIcons/Water or fluids.png", UriKind.RelativeOrAbsolute))},
        };
    }
}