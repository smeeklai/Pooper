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
    public partial class AdditionalHealthInfomation : PhoneApplicationPage
    {
        public AdditionalHealthInfomation()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            temGender_picker.SelectionChanged += temGender_picker_SelectionChanged;
        }

        private void temGender_picker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (temGender_picker.SelectedItem.ToString().Contains("Female"))
            {
                femailOnly_border.Visibility = Visibility.Visible;
                femaleOnly_stackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                femailOnly_border.Visibility = Visibility.Collapsed;
                femaleOnly_stackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void submit_button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(temWeight_textBox.Text) || String.IsNullOrWhiteSpace(temHeight_textBox.Text) ||
                String.IsNullOrWhiteSpace(temAge_textBox.Text))
            {
                MessageBox.Show("Weight, height or age cannot be empty", AppResources.Warning, MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("test");
                /*double guestWeight = Convert.ToDouble(temWeight_textBox.Text);
                double guestHeight = Convert.ToDouble(temHeight_textBox.Text);
                int guestAge = Convert.ToInt32(temAge_textBox.Text);
                string guestGender = temGender_picker.SelectedItem.ToString();
                NavigationService.Navigate(new Uri("/ResultPage.xaml?weight=" + guestWeight + "?height=" + guestHeight + "?age=" + guestAge
                    + "?gender=" + guestGender, UriKind.Relative));*/
            }
        }


    }
}