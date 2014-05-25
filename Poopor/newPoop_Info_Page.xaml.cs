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
using System.Threading.Tasks;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO.IsolatedStorage;
using System.IO;

namespace Poopor
{
    public partial class Poop_info_page : PhoneApplicationPage
    {
        private string poopImgName;
        public Poop_info_page()
        {
            InitializeComponent();

            colorPicker.ItemsSource = ColorExtensions.AccentColors();
            shapePicker.ItemsSource = ShapeData.ShapeNames();

        }

        private bool IsInit;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("poopColor", out poopColor);
            isMelena = Convert.ToBoolean(NavigationContext.QueryString["melenaResult"]);
            havingMedicines = Convert.ToBoolean(NavigationContext.QueryString["havingMedicines"]);
            if (!IsInit)
            {
                colorPicker.SelectedItem = poopColor;
                IsInit = true;
            }

            painLevel_slider.ValueChanged += painLevel_slider_ValueChanged;
            blood_amount_slider.ValueChanged += blood_amount_slider_ValueChanged;
            colorPicker.SelectionChanged += colorPicker_SelectionChanged;
            shapePicker.SelectionChanged += shapePicker_SelectionChanged;
        }

        private async void newPoop_submit_button_Click(object sender, RoutedEventArgs e)
        {
            poopColor = colorPicker.SelectedItem.ToString();
            string shape = shapePicker.SelectedItem.ToString();
            string painLevel = painLevel_dictionary[painLevel_slider.Value];
            string bloodAmount = bloodAmount_dictionary[blood_amount_slider.Value];
            DateTime userPoopStoredDateTime = DateTime.Now;

            if (SessionManagement.IsLoggedIn() == false)
            {
                MessageBox.Show("Please answer us several health questions first. This can be avoided by siging up an account", "Health Infomation Required",
                    MessageBoxButton.OK);
                NavigationService.Navigate(new Uri("/AdditionalHealthInfomation.xaml?poopColor=" + poopColor + "&shape=" + shape + "&painLevel=" + painLevel
                    + "&bloodAmount=" + bloodAmount + "&melenaResult=" + isMelena + "&havingMedicines=" + havingMedicines
                    + "&userPoopStoredDateTime=" + userPoopStoredDateTime, UriKind.Relative));
            }
            else
            {
                SystemFunctions.SetProgressIndicatorProperties(true);
                SystemTray.ProgressIndicator.Text = "Analysing data...";

                if (!SQLiteFunctions.IsResultCriteriaInitialized())
                {
                    await System.Threading.Tasks.Task.Run(() => SystemFunctions.InitializeResultCriterias());
                }

                var userInfo = sqliteFunctions.GetUserInfo(SessionManagement.GetEmail());
                int userAge = Convert.ToInt32(DateTime.Now.Subtract(userInfo.DOB).TotalDays / 360);
                var result = await new FecesAnalyzer().analyzeData(poopColor, shape, painLevel, bloodAmount, userInfo.Height, userInfo.Weight, userInfo.Gender, userAge,
                    userInfo.HealthInfo1, userInfo.HealthInfo2, userInfo.HealthInfo3, userInfo.HealthInfo4, userInfo.HealthInfo5, isMelena, havingMedicines);

                List<ResultAndRecommendationDictionary> serializedResult = SystemFunctions.SerializeUserResultAndRecommendationData(result);
                SessionManagement.StoreUserLastestResultsAndRecommendation(serializedResult);

                SystemTray.ProgressIndicator.Text = "Storing data...";
                List<string> necessaryInfo = result["NecessaryInfo"];
                Boolean constipation = Convert.ToBoolean(necessaryInfo[3]);
                Boolean diarrhea = Convert.ToBoolean(necessaryInfo[4]);
                poopImgName = NavigationContext.QueryString["poopImgName"];
                Boolean insertationResult = await InsertNewPoopData(poopColor, shape, painLevel, bloodAmount, havingMedicines, poopImgName, userPoopStoredDateTime,
                    diarrhea, constipation, isMelena);

                SystemFunctions.SetProgressIndicatorProperties(false);

                if (insertationResult == true)
                {
                    Boolean isAdditionalAskingNeeded = Convert.ToBoolean(necessaryInfo[0]);
                    if (isAdditionalAskingNeeded == false)
                        NavigationService.Navigate(new Uri("/ResultPage.xaml", UriKind.Relative));
                    else
                        NavigationService.Navigate(new Uri("/AdditionalHealthInfomation2.xaml", UriKind.Relative));
                }
            }
        }

        private async Task<Boolean> InsertNewPoopData(string poopColor, string poopShape, string painLv, string bloodAmt, Boolean havingMedicines,
            string poopImageName, DateTime userPoopStoredDateTime, Boolean diarrhea, Boolean constipation, Boolean isMelena)
        {
            //If a user has no internet connection, no need to store data into Azure
            azureResult = NetworkInterface.GetIsNetworkAvailable() ? false : true;

            if (azureResult == false)
                azureResult = await InsertNewPoopDataToAzure(poopColor, poopShape, painLv, bloodAmt, havingMedicines, poopImageName, userPoopStoredDateTime,
                    diarrhea, constipation, isMelena);
            if (azureResult == true && sqliteResult == false)
                sqliteResult = InsertNewPoopDataToSQLite(poopColor, poopShape, painLv, bloodAmt, havingMedicines, poopImageName, userPoopStoredDateTime,
                    diarrhea, constipation, isMelena);
            if (azureResult && sqliteResult == true)
            {
                Debug.WriteLine("Poop data is stored");
                return true;
            }
            return false;
        }

        private async Task<Boolean> InsertNewPoopDataToAzure(string poopColor, string poopShape, string painLv, string bloodAmt, Boolean havingMedicines,
            string poopImageName, DateTime userPoopStoredDateTime, Boolean diarrhea, Boolean constipation, Boolean isMelena)
        {
            Debug.WriteLine("Username : " + SessionManagement.GetEmail());
            var containerName = SystemFunctions.RemoveSpecialCharacters(SessionManagement.GetEmail());
            Boolean result = await new AzureFunctions().InsertDataAsync(new Poop_Table_Azure()
            {
                Email = SessionManagement.GetEmail(),
                Color = poopColor,
                Shape = poopShape,
                Pain_Level = painLv,
                Blood_Amount = bloodAmt,
                Having_Medicines = havingMedicines,
                Poop_Picture_Name = poopImageName,
                Date_Time = userPoopStoredDateTime,
                Diarrhea = diarrhea,
                Constipation = constipation,
                MelenaPoop = isMelena,
                ImageUri = "",
                SasQueryString = "",
                ContainerName = containerName
            });

            if (result == false)
            {
                MessageBox.Show("An network error has ouccured. Please submit again", "Try again", MessageBoxButton.OK);
            }
            return result;
        }

        private Boolean InsertNewPoopDataToSQLite(string poopColor, string poopShape, string painLv, string bloodAmt, Boolean havingMedicines,
            string poopImageName, DateTime userPoopStoredDateTime, Boolean diarrhea, Boolean constipation, Boolean isMelena)
        {
            Boolean result = new SQLiteFunctions().InsertData(new Poop_Table_SQLite()
            {
                Username = SessionManagement.GetEmail(),
                Color = poopColor,
                Shape = poopShape,
                Pain_Level = painLv,
                Blood_Amount = bloodAmt,
                Having_Medicines = havingMedicines,
                Poop_Picture_Name = poopImageName,
                Date_Time = userPoopStoredDateTime,
                Diarrhea = diarrhea,
                Constipation = constipation,
                MelenaPoop = isMelena
            });
            if (result == false)
            {
                MessageBox.Show("An system error has ouccured. Please submit again", "Try again", MessageBoxButton.OK);
            }
            return result;
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
            if (e.NewValue == 1)
            {
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/None.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_Normal;
            }
            else if (e.NewValue <= 2)
            {
                painLevel_slider.Value = 2;
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/Mild.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_Mild;
            }
            else if (e.NewValue <= 3)
            {
                painLevel_slider.Value = 3;
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/Moderate.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_Moderate;
            }
            else if (e.NewValue <= 4)
            {
                painLevel_slider.Value = 4;
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/Severe.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_Severe;
            }
            else if (e.NewValue <= 5)
            {
                painLevel_slider.Value = 5;
                pain_level_picture.Source = new BitmapImage(new Uri("/Assets/img/painLevel/Worst.png", UriKind.RelativeOrAbsolute));
                pain_level_description.Text = AppResources.PainLevel_worst;
            }
        }

        void blood_amount_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == 1)
            {
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/None.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_Normal;
            }
            else if (e.NewValue <= 2)
            {
                blood_amount_slider.Value = 2;
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/Little blood.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_Mild;
            }
            else if (e.NewValue <= 3)
            {
                blood_amount_slider.Value = 3;
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/Medium blood.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_Moderate;
            }
            else if (e.NewValue <= 4)
            {
                blood_amount_slider.Value = 4;
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/Much blood.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_Severe;
            }
            else if (e.NewValue <= 5)
            {
                blood_amount_slider.Value = 5;
                blood_amount_picture.Source = new BitmapImage(new Uri("/Assets/img/bloodAmount/A lot of blood.png", UriKind.RelativeOrAbsolute));
                blood_amount_description.Text = AppResources.PainLevel_worst;
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (!SessionManagement.IsLoggedIn())
            {
                SessionManagement.Logout();
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private string poopColor;
        private Boolean isMelena = false;
        private Boolean havingMedicines = false;
        private AzureFunctions azureFunctions = new AzureFunctions();
        private SQLiteFunctions sqliteFunctions = new SQLiteFunctions();
        private Boolean azureResult = false;
        private Boolean sqliteResult = false;
        private Dictionary<double, string> painLevel_dictionary = new Dictionary<double, string>(){
            {1, "None"},
            {2, "Mild"},
            {3, "Moderate"},
            {4, "Severe"},
            {5, "Worst"}
        };
        private Dictionary<double, string> bloodAmount_dictionary = new Dictionary<double, string>(){
            {1, "None"},
            {2, "Little blood"},
            {3, "Medium blood"},
            {4, "Much blood"},
            {5, "A lot of blood"}
        };
    }
}