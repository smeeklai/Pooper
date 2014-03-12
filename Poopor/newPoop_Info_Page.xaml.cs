using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Poopor.Resources;
using Poopor.Data;

namespace Poopor
{
    public partial class Poop_info_page : PhoneApplicationPage
    {
        private Dictionary<double, string> painLevel_dictionary = new Dictionary<double, string>(){
            {1, "no hurt"},
            {2, "mild"},
            {3, "moderate"},
            {4, "severe"},
            {5, "worst"}
        };
        private Dictionary<double, string> bloodAmount_dictionary = new Dictionary<double, string>(){
            {1, "no blood"},
            {2, "little blood"},
            {3, "medium blood"},
            {4, "much blood"},
            {5, "a lot of blood"}
        };

        public Poop_info_page()
        {
            InitializeComponent();

            colorPicker.ItemsSource = ColorExtensions.AccentColors();
            shapePicker.ItemsSource = ShapeData.ShapeNames();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            painLevel_slider.ValueChanged += painLevel_slider_ValueChanged;
            blood_amount_slider.ValueChanged += blood_amount_slider_ValueChanged;
            colorPicker.SelectionChanged += colorPicker_SelectionChanged;
            shapePicker.SelectionChanged += shapePicker_SelectionChanged;
        }

        private void newPoop_submit_button_Click(object sender, RoutedEventArgs e)
        {
            string color = colorPicker.SelectedItem.ToString();
            string shape = shapePicker.SelectedItem.ToString();
            string painLevel = painLevel_dictionary[painLevel_slider.Value];
            string bloodAmount = bloodAmount_dictionary[blood_amount_slider.Value];

            if (SessionManagement.IsLoggedIn() == false)
            {
                MessageBox.Show("Please answer us several health questions first. This can be avoided by siging up an account", "Health Infomation Required", MessageBoxButton.OK);
                NavigationService.Navigate(new Uri("/AdditionalHealthInfomation.xaml?color=" + color + "?shape=" + shape + "?painLevel=" + painLevel
                    + "?bloodAmount=" + bloodAmount, UriKind.Relative));
            }
            {
                //---------------------------Call Bank & Fern Method here----------------------------------

                Poop_Table_SQLite newPoopData = new Poop_Table_SQLite();
                newPoopData.Email = SessionManagement.GetEmail();
                newPoopData.Color = color;
                newPoopData.Shape = shape;
                newPoopData.Pain_Level = painLevel;
                newPoopData.Blood_Amount = bloodAmount;

                NavigationService.Navigate(new Uri("/ResultPage.xaml", UriKind.Relative));
            }
            
        }

        void shapePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            Debug.WriteLine(picker.SelectedItem.ToString());
            shapeResult.Source = ShapeTypeToImg.ConvertShapeStringToImg(picker.SelectedItem.ToString());
            shapeResult_text.Text = picker.SelectedItem.ToString();
        }

        void colorPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            colorResult.Fill = AccentColorNameToBrush.ConvertStringToSolidColorBrush(picker.SelectedItem.ToString());
            colorResult_text.Text = picker.SelectedItem.ToString();
        }

        private void painLevel_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == 1) {
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/nohurt.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_Normal;
            }
            else if (e.NewValue <= 2) {
                painLevel_slider.Value = 2;
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/mild.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_Mild;
            }
            else if (e.NewValue <= 3) {
                painLevel_slider.Value = 3;
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/Moderate.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_Moderate;
            }
            else if (e.NewValue <= 4) {
                painLevel_slider.Value = 4;
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/Severe.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_Severe;
            }
            else if (e.NewValue <= 5) {
                painLevel_slider.Value = 5;
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/worst.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_worst;
            }
        }

        void blood_amount_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == 1)
            {
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/noblood.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_Normal;
            }
            else if (e.NewValue <= 2)
            {
                blood_amount_slider.Value = 2;
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/littleblood.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_Mild;
            }
            else if (e.NewValue <= 3)
            {
                blood_amount_slider.Value = 3;
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/mediumblood.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_Moderate;
            }
            else if (e.NewValue <= 4)
            {
                blood_amount_slider.Value = 4;
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/muchblood.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_Severe;
            }
            else if (e.NewValue <= 5)
            {
                blood_amount_slider.Value = 5;
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/alotofblood.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_worst;
            }
        }
    }
}