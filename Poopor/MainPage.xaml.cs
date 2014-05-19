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
using System.Diagnostics;
using SQLite;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Scheduler;
using System.ComponentModel;
using System.Threading;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows.Threading;
using System.IO.IsolatedStorage;

namespace Poopor
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string userHealth;
        private List<Poop_Table_SQLite> userLastestPoopDataInSQLite = null;
        private MobileServiceCollection<Poop_Table_Azure, Poop_Table_Azure> userLastestPoopDataInAzure;
        private Poop_Table_SQLite userLastestPoopRecordInSqlite = null;
        private Poop_Table_Azure userLastestPoopRecordInAzure = null;
        private Dictionary<string, List<string>> userLastestResultsAndRecommendation;
        private int isUpdateNeeded;
        private AzureFunctions azureFunctions = new AzureFunctions();
        private SQLiteFunctions sqliteFunctions = new SQLiteFunctions();
        private DispatcherTimer timer = new DispatcherTimer();

        public MainPage()
        {
            InitializeComponent();
            if (SessionManagement.IsLoggedIn())
            {
                userLastestResultsAndRecommendation = SessionManagement.GetUserLastestResultsAndRecommendation();
                if (userLastestResultsAndRecommendation != null)
                {
                    lastRecommendation_button.Opacity = 100;
                    lastRecommendation_button.Click += lastRecommendation_button_Click;
                    List<string> necessaryInfo = null;
                    if (userLastestResultsAndRecommendation.TryGetValue("NecessaryInfo", out necessaryInfo))
                    {
                        userHealth = necessaryInfo[1];
                        if (!userHealth.Equals("none"))
                            AdaptDashboardToUserCancerSign(userHealth);
                    }
                    else
                        Debug.WriteLine("No UserCancerSign key");
                }
                else
                    Debug.WriteLine("No user lastest result and recommendation");

                buildApplicationBar();
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (SessionManagement.IsLoggedIn())
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    DateTime userLatestPoopTimeInAzure = SessionManagement.GetUserLatestPoopTime();
                    if (!userLatestPoopTimeInAzure.Equals(new DateTime()))
                    {
                        SystemTray.ProgressIndicator = new ProgressIndicator();
                        SystemTray.ProgressIndicator.Text = GetLastUpdatedTimeInText(userLatestPoopTimeInAzure);
                        SystemTray.ProgressIndicator.IsVisible = true;

                        timer.Interval = TimeSpan.FromMilliseconds(3000);

                        timer.Tick += (sender, args) =>
                        {
                            try
                            {
                                SystemTray.ProgressIndicator.IsVisible = false;
                                timer.Stop();
                            }
                            catch (Exception c)
                            {
                                Debug.WriteLine(c.Message);
                            }
                        };

                        timer.Start();
                    }
                }
                else
                {
                    userLastestPoopDataInSQLite = sqliteFunctions.GetUserPoopData(SessionManagement.GetEmail());
                    userLastestPoopDataInAzure = await azureFunctions.GetUserPoopDataInAzure(SessionManagement.GetEmail());
                    if (userLastestPoopDataInAzure != null)
                    {
                        if (userLastestPoopDataInAzure.Count != 0 || userLastestPoopDataInSQLite.Count != 0)
                        {
                            if (userLastestPoopDataInAzure.Count == 0)
                            {
                                Debug.WriteLine("No data in Azure, sync data from SQLite to Azure");
                                isUpdateNeeded = 1;
                            }
                            else if (userLastestPoopDataInSQLite.Count == 0)
                            {
                                Debug.WriteLine("No data in SQLite, sync data from Azure to SQLite");
                                isUpdateNeeded = -1;
                            }
                            else
                            {
                                userLastestPoopRecordInAzure = userLastestPoopDataInAzure.Last();
                                userLastestPoopRecordInSqlite = userLastestPoopDataInSQLite.Last();
                                DateTime dateTimeInSQLite = userLastestPoopRecordInSqlite.Date_Time;
                                DateTime dateTimeInAzure = userLastestPoopRecordInAzure.Date_Time;
                                isUpdateNeeded = DateTime.Compare(new DateTime(dateTimeInSQLite.Year, dateTimeInSQLite.Month, dateTimeInSQLite.Day, dateTimeInSQLite.Hour, dateTimeInSQLite.Minute, dateTimeInSQLite.Second),
                                    new DateTime(dateTimeInAzure.Year, dateTimeInAzure.Month, dateTimeInAzure.Day, dateTimeInAzure.Hour, dateTimeInAzure.Minute, dateTimeInAzure.Second));
                                Debug.WriteLine("Lastest time in Sqlite: " + userLastestPoopRecordInSqlite.Date_Time);
                                Debug.WriteLine("Lastest time in azure: " + userLastestPoopRecordInAzure.Date_Time);
                            }


                            Debug.WriteLine(isUpdateNeeded);
                            if (isUpdateNeeded == 0)
                            {
                                SystemTray.ProgressIndicator = new ProgressIndicator();
                                SystemTray.ProgressIndicator.Text = "Data is up-to-date";
                                SystemTray.ProgressIndicator.IsVisible = true;

                                timer.Interval = TimeSpan.FromMilliseconds(3000);

                                timer.Tick += (sender, args) =>
                                {
                                    try
                                    {
                                        SystemTray.ProgressIndicator.IsVisible = false;
                                        timer.Stop();
                                    }
                                    catch (Exception b)
                                    {
                                        Debug.WriteLine(b.Message);
                                    }
                                };

                                timer.Start();
                            }
                            else
                            {
                                StartSyncUserLastestData();
                            }
                        }
                    }
                }
            }
        }

        private void StartSyncUserLastestData()
        {
            BackgroundWorker backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);
            backroungWorker.RunWorkerAsync();
        }

        private void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (isUpdateNeeded > 0)
            {
                Debug.WriteLine("Data in SQLite is newer");
                SyncDataToAzure();
                this.Dispatcher.BeginInvoke(() =>
                {
                    SystemFunctions.SetProgressIndicatorProperties(true);
                    SystemTray.ProgressIndicator.Text = "Syncing data...";
                }
                );
            }
            else
            {
                Debug.WriteLine("Data in Azure is newer");
                SyncDataToSQLite();
                this.Dispatcher.BeginInvoke(() =>
                {
                    SystemFunctions.SetProgressIndicatorProperties(true);
                    SystemTray.ProgressIndicator.Text = "Syncing data...";
                }
                );
            }
        }

        private void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("Finished");
            SystemTray.ProgressIndicator.IsVisible = false;
        }

        private async void SyncDataToSQLite()
        {
            MobileServiceCollection<Poop_Table_Azure, Poop_Table_Azure> result;
            if (userLastestPoopRecordInSqlite == null)
            {
                result = userLastestPoopDataInAzure;
            }
            else
            {
                result = await azureFunctions.GetUserPoopDataAfterInputDate(SessionManagement.GetEmail(), userLastestPoopRecordInSqlite.Date_Time);
            }
            Debug.WriteLine("There are {0} new data in Azure", result.Count);
            foreach (var item in result)
            {
                Boolean resultOfInsertation = false;
                while (resultOfInsertation == false)
                {
                    resultOfInsertation = sqliteFunctions.InsertData(new Poop_Table_SQLite()
                    {
                        Username = SessionManagement.GetEmail(),
                        Color = item.Color,
                        Shape = item.Shape,
                        Blood_Amount = item.Blood_Amount,
                        Pain_Level = item.Pain_Level,
                        Having_Medicines = item.Having_Medicines,
                        Poop_Picture_Name = item.Poop_Picture_Name,
                        Date_Time = item.Date_Time,
                        Diarrhea = item.Diarrhea,
                        Constipation = item.Constipation,
                        MelenaPoop = item.MelenaPoop
                    });
                    Debug.WriteLine("SQLite: " + resultOfInsertation + "Datetime " + item.Date_Time);
                }
            }
            Debug.WriteLine("Insert Finish");
        }

        private async void SyncDataToAzure()
        {
            List<Poop_Table_SQLite> poopDataToBeAdded = new List<Poop_Table_SQLite>();
            if (userLastestPoopRecordInAzure == null)
            {
                poopDataToBeAdded = userLastestPoopDataInSQLite;
            }
            else
            {
                userLastestPoopDataInSQLite.Reverse();
                foreach (var item in userLastestPoopDataInSQLite)
                {
                    if (item.Date_Time > userLastestPoopRecordInAzure.Date_Time)
                        poopDataToBeAdded.Add(item);
                    else
                        break;
                }
                poopDataToBeAdded.Reverse();
            }
            Debug.WriteLine("Poop data to be added " + poopDataToBeAdded.Count());
            foreach (var item in poopDataToBeAdded)
            {
                Boolean resultOfInsertation = false;
                while (resultOfInsertation == false)
                {
                    resultOfInsertation = await azureFunctions.InsertDataAsync(new Poop_Table_Azure()
                    {
                        Email = SessionManagement.GetEmail(),
                        Color = item.Color,
                        Shape = item.Shape,
                        Blood_Amount = item.Blood_Amount,
                        Pain_Level = item.Pain_Level,
                        Having_Medicines = item.Having_Medicines,
                        Poop_Picture_Name = item.Poop_Picture_Name,
                        Date_Time = item.Date_Time,
                        Diarrhea = item.Diarrhea,
                        Constipation = item.Constipation,
                        MelenaPoop = item.MelenaPoop,
                        ImageUri = "",
                        SasQueryString = ""
                    });
                    Debug.WriteLine("Azure: " + resultOfInsertation + " Datetime: " + item.Date_Time);
                }
            }
            Debug.WriteLine("Insert Finish");
        }

        private void AdaptDashboardToUserCancerSign(string userHealth)
        {
            List<string> userCancerSignMsg = userLastestResultsAndRecommendation["UserCancerSignMsg"];
            userCancerSignMsg = SystemFunctions.SortByLength(userCancerSignMsg).ToList();
            if (userHealth.Equals("general"))
            {
                goodHealthInfo_grid.Visibility = System.Windows.Visibility.Collapsed;
                more_axiousSigns_text.Visibility = System.Windows.Visibility.Collapsed;
                suggestion_textBlock.Text = "Be careful with your health";
                representation_image.Source = new BitmapImage(new Uri("/Assets/img/risk/genrisk.png", UriKind.RelativeOrAbsolute));
                healthInfo_grid.Visibility = System.Windows.Visibility.Visible;
                SolidColorBrush newBgColor = new SolidColorBrush();
                newBgColor.Color = Color.FromArgb(255, 241, 145, 32);
                healthInfo_grid.Background = newBgColor;
                header_textBlock.Text = "Detected! marginal risks of colon-rectum cancer";
                try
                {
                    if (userCancerSignMsg != null)
                    {
                        if (userCancerSignMsg[0] != null)
                            moreInfo_textBlock1.Text = "- " + userCancerSignMsg[0];
                        if (userCancerSignMsg[1] != null)
                            moreInfo_textBlock2.Text = "- " + userCancerSignMsg[1];
                        if (userCancerSignMsg[2] != null)
                            moreInfo_textBlock3.Text = "- " + userCancerSignMsg[2];
                    }
                }
                catch (ArgumentOutOfRangeException error)
                {
                    Debug.WriteLine(error.Message);
                }
            }
            else if (userHealth.Equals("anxious"))
            {
                goodHealthInfo_grid.Visibility = System.Windows.Visibility.Collapsed;
                healthInfo_grid.Visibility = System.Windows.Visibility.Visible;
                try
                {
                    if (userCancerSignMsg != null)
                    {
                        if (userCancerSignMsg[0] != null)
                            moreInfo_textBlock1.Text = "- " + userCancerSignMsg[0];
                        if (userCancerSignMsg[1] != null)
                            moreInfo_textBlock2.Text = "- " + userCancerSignMsg[1];
                        if (userCancerSignMsg[2] != null)
                            moreInfo_textBlock3.Text = "- " + userCancerSignMsg[2];
                    }
                }
                catch (ArgumentOutOfRangeException error)
                {

                }
            }
        }

        private void newPoop_button_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            NavigationService.Navigate(new Uri("/PicturePage.xaml", UriKind.Relative));
        }

        private string GetLastUpdatedTimeInText(DateTime poopDateTime)
        {
            string result = null;
            TimeSpan timeSpanDifference = DateTime.Now.Subtract(poopDateTime);
            if (timeSpanDifference.Days >= 1)
            {
                if (timeSpanDifference.Days == 1)
                    result = "Last synced " + timeSpanDifference.Days + " day ago";
                else
                    result = "Last synced " + timeSpanDifference.Days + " days ago";
            }
            else if (timeSpanDifference.Hours >= 1)
            {
                if (timeSpanDifference.Hours == 1)
                    result = "Last synced " + timeSpanDifference.Hours + " hour ago";
                else
                    result = "Last synced " + timeSpanDifference.Hours + " hours ago";
            }
            else if (timeSpanDifference.Minutes >= 1)
            {
                if (timeSpanDifference.Minutes == 1)
                    result = "Last synced " + timeSpanDifference.Minutes + " minute ago";
                else
                    result = "Last synced " + timeSpanDifference.Minutes + " minutes ago";
            }
            else
            {
                if (timeSpanDifference.Seconds == 1)
                    result = "Last synced " + timeSpanDifference.Seconds + " second ago";
                else
                    result = "Last synced " + timeSpanDifference.Seconds + " seconds ago";
            }
            return result;
        }

        private void buildApplicationBar()
        {
            //Initialize ApplicationBar
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 0.8;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            //Initialize ApplicationBarMenuItem
            ApplicationBarMenuItem logoutAppBar = new ApplicationBarMenuItem();
            logoutAppBar.Text = AppResources.AppBarLogout;
            logoutAppBar.Click += new EventHandler(logoutAppBar_Click);

            //Add menu item
            ApplicationBar.MenuItems.Add(logoutAppBar);
        }

        private async void logoutAppBar_Click(object sender, EventArgs e)
        {
            SessionManagement.Logout();
            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void OnFlick(object sender, FlickGestureEventArgs e)
        {
            if (userHealth.Equals("anxious"))
            {
                // User flicked towards right
                if (e.HorizontalVelocity > 0)
                {
                    NavigationService.Navigate(new Uri("/ColonCancerMsgPage.xaml", UriKind.Relative));
                }
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (SessionManagement.IsLoggedIn())
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
                Application.Current.Terminate();
            }
        }

        private void lastRecommendation_button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ResultPage.xaml", UriKind.Relative));
        }

        private void getReport_button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ReportPage.xaml", UriKind.Relative));
        }
    }
}