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
        private string poopColor;
        private string poopShape;
        private string bloodAmount;
        private string painLevel;
        private Boolean isMelena = false;
        private Boolean havingMedicines = false;

        public AdditionalHealthInfomation()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("poopColor", out poopColor);
            NavigationContext.QueryString.TryGetValue("shape", out poopShape);
            NavigationContext.QueryString.TryGetValue("painLevel", out bloodAmount);
            NavigationContext.QueryString.TryGetValue("bloodAmount", out painLevel);
            isMelena = Convert.ToBoolean(NavigationContext.QueryString["melenaResult"]);
            havingMedicines = Convert.ToBoolean(NavigationContext.QueryString["havingMedicines"]);
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

        private async void submit_button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(temWeight_textBox.Text) || String.IsNullOrWhiteSpace(temHeight_textBox.Text) ||
                String.IsNullOrWhiteSpace(temAge_textBox.Text))
            {
                MessageBox.Show("Weight, height or age cannot be empty", AppResources.Warning, MessageBoxButton.OK);
            }
            else
            {
                SystemFunctions.SetProgressIndicatorProperties(true);
                SystemTray.ProgressIndicator.Text = "Analyzing...";
                double userWeight = Convert.ToDouble(temWeight_textBox.Text);
                double userHeight = Convert.ToDouble(temHeight_textBox.Text);
                int userAge = Convert.ToInt32(temAge_textBox.Text);
                string userGender = temGender_picker.SelectedItem.ToString();
                Boolean userHealthInfo1 = (Boolean)temHealthInfo_checkBox1.IsChecked;
                Boolean userHealthInfo2 = (Boolean)temHealthInfo_checkBox2.IsChecked;
                Boolean userHealthInfo3 = (Boolean)temHealthInfo_checkBox3.IsChecked;
                Boolean userHealthInfo4 = (Boolean)temHealthInfo_checkBox4.IsChecked;
                Boolean userHealthInfo5 = userGender == "Female" ? (Boolean)temHealthInfo_checkBox5.IsChecked : false;
                if (!SQLiteFunctions.IsResultCriteriaInitialized())
                {
                    await SystemFunctions.InitializeResultCriterias();
                }
                var result = await new FecesAnalyzer().analyzeData(poopColor, poopShape, painLevel, bloodAmount, userWeight, userHeight, userGender, userAge,
                    userHealthInfo1, userHealthInfo2, userHealthInfo3, userHealthInfo4, userHealthInfo5, isMelena, havingMedicines);
                //SessionManagement.StoreUserLastestResultsAndRecommendation(result);
                SystemFunctions.SetProgressIndicatorProperties(false);

                Boolean isAdditionalAskingNeeded = result.ContainsKey("IsGoAsk") ? Convert.ToBoolean(result["IsGoAsk"]) : false;
                if (isAdditionalAskingNeeded == false)
                    NavigationService.Navigate(new Uri("/ResultPage.xaml", UriKind.Relative));
                else
                    NavigationService.Navigate(new Uri("/AdditionalHealthInfomation2.xaml", UriKind.Relative));
            }
        }


    }
}