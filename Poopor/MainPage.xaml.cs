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

namespace Poopor
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string userHealth;
        private BackgroundWorker backroungWorker;
        private Poop_Table_SQLite userLastestpoopRecordInSqlite;
        private MobileServiceCollection<Poop_Table_Azure, Poop_Table_Azure> userLastestPoopDataInAzure;
        private Poop_Table_Azure userLastestPoopRecordInAzure;
        private int isUpdateNeeded;
        private AzureFunctions azureFunctions = new AzureFunctions();
        private SQLiteFunctions sqliteFunctions = new SQLiteFunctions();
        private DispatcherTimer timer = new DispatcherTimer();
        /*private Dictionary<string, object> userLastestResultsAndRecommendation = new Dictionary<string, object>()
        {
            {"usercancersign", "normal"},
        };*/

        public MainPage()
        {
            InitializeComponent();
            /*List<string> test = new List<string>();
            test.Add("test1");
            test.Add("test2");
            test.Add("- Your first-relative relationship members have been diagnosed with FAP or HNPCC");
            userLastestResultsAndRecommendation.Add("userCancerSignMsg", test);
            var userLastestResultsAndRecommendation = SessionManagement.GetUserLastestResultsAndRecommendation();
            if (userLastestResultsAndRecommendation != null)
            {
                object userHealthObj = null;
                if (userLastestResultsAndRecommendation.TryGetValue("UserCancerSign", out userHealthObj))
                {
                    userHealth = userHealthObj as string;
                    if (!userHealth.Contains("normal"))
                        AdaptInterfaceToUserHealth(userHealth);
                }
                else
                    Debug.WriteLine("failed");
            } else
                Debug.WriteLine("no existed");*/
            buildApplicationBar();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (SessionManagement.IsLoggedIn())
            {
                var userLastestPoopDataInSQLite = sqliteFunctions.GetUserPoopData(SessionManagement.GetEmail());
                if (userLastestPoopDataInSQLite.Count != 0)
                {
                    userLastestpoopRecordInSqlite = userLastestPoopDataInSQLite.Last();
                    if (!NetworkInterface.GetIsNetworkAvailable())
                    {
                        SystemTray.ProgressIndicator = new ProgressIndicator();
                        SystemTray.ProgressIndicator.Text = GetLastUpdatedTimeInText(userLastestpoopRecordInSqlite.Date_Time);
                        SystemTray.ProgressIndicator.IsVisible = true;

                        timer.Interval = TimeSpan.FromMilliseconds(3000);

                        timer.Tick += (sender, args) =>
                        {
                            SystemTray.ProgressIndicator.IsVisible = false;
                            timer.Stop();
                        };

                        timer.Start();
                    }
                    else
                    {
                        isUpdateNeeded = DateTime.Compare(userLastestpoopRecordInSqlite.Date_Time, userLastestPoopRecordInAzure.Date_Time);
                        Debug.WriteLine("Lastest time in Sqlite" + userLastestpoopRecordInSqlite.Date_Time);
                        Debug.WriteLine("Lastest time in azure" + userLastestPoopRecordInAzure.Date_Time);
                        Debug.WriteLine(isUpdateNeeded);
                        if (isUpdateNeeded == 0)
                        {
                            SystemTray.ProgressIndicator = new ProgressIndicator();
                            SystemTray.ProgressIndicator.Text = "Data is up-to-date";
                            SystemTray.ProgressIndicator.IsVisible = true;

                            timer.Interval = TimeSpan.FromMilliseconds(3000);

                            timer.Tick += (sender, args) =>
                            {
                                SystemTray.ProgressIndicator.IsVisible = false;
                                timer.Stop();
                            };

                            timer.Start();
                        }
                        else
                        {
                            userLastestPoopDataInAzure = await azureFunctions.GetUserPoopDataInAzure(SessionManagement.GetEmail());
                            userLastestPoopRecordInAzure = userLastestPoopDataInAzure.Last();
                            StartSyncUserLastestData();
                        }
                    }
                }
            }
        }

        private void StartSyncUserLastestData()
        {
            backroungWorker = new BackgroundWorker();
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
            var result = await azureFunctions.GetUserPoopDataAfterInputDate(SessionManagement.GetEmail(), userLastestpoopRecordInSqlite.Date_Time);
            Debug.WriteLine("There are {0} new data in Azure", result.Count);
            foreach (var item in result)
            {
                Boolean resultOfInsertation = false;
                while (resultOfInsertation == false)
                {
                    resultOfInsertation = await sqliteFunctions.InsertData(new Poop_Table_SQLite()
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
                        MelenaPoop = item.MelenaPoop
                    });
                    Debug.WriteLine("SQLite: " + resultOfInsertation + "Datetime " + item.Date_Time);
                }
            }
            Debug.WriteLine("Insert Finish");
        }

        private async void SyncDataToAzure()
        {
            var tempResult = sqliteFunctions.GetUserPoopData(SessionManagement.GetEmail());
            var poopDataToBeAdded = new List<Poop_Table_SQLite>();
            tempResult.Reverse();
            foreach (var item in tempResult)
            {
                if (item.Date_Time > userLastestPoopRecordInAzure.Date_Time)
                    poopDataToBeAdded.Add(item);
                else
                    break;
            }
            poopDataToBeAdded.Reverse();
            Debug.WriteLine("Poop data to be added " + poopDataToBeAdded.Count());
            foreach (var item in poopDataToBeAdded)
            {
                Boolean resultOfInsertation = false;
                while (resultOfInsertation == false)
                {
                    resultOfInsertation = await azureFunctions.InsertData(new Poop_Table_Azure()
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
                        MelenaPoop = item.MelenaPoop
                    });
                    Debug.WriteLine("Azure: " + resultOfInsertation + " Datetime: " + item.Date_Time);
                }
            }
            Debug.WriteLine("Insert Finish");
        }

        /* private void AdaptInterfaceToUserHealth(string userHealth)
         {
             if (userHealth.Contains("suspicious"))
             {
                 List<string> userCancerSignMsg = userLastestResultsAndRecommendation["userCancerSignMsg"] as List<string>;
                 goodHealthInfo_grid.Visibility = System.Windows.Visibility.Collapsed;
                 more_axiousSigns_text.Visibility = System.Windows.Visibility.Collapsed;
                 suggestion_textBlock.Text = "Be careful with your health";
                 representation_image.Source = new BitmapImage(new Uri("/Assets/img/risk/genrisk.png", UriKind.RelativeOrAbsolute));
                 healthInfo_grid.Visibility = System.Windows.Visibility.Visible;
                 SolidColorBrush newBgColor = new SolidColorBrush();
                 newBgColor.Color = Color.FromArgb(255, 241, 145, 32);
                 healthInfo_grid.Background = newBgColor;
                 header_textBlock.Text = "Detected! general signs of colon-rectum cancer";
                 try
                 {
                     if (userCancerSignMsg[0] != null)
                         moreInfo_textBlock1.Text = userCancerSignMsg[0];
                     if (userCancerSignMsg[1] != null)
                         moreInfo_textBlock2.Text = userCancerSignMsg[1];
                     if (userCancerSignMsg[2] != null)
                         moreInfo_textBlock3.Text = userCancerSignMsg[2];
                 }
                 catch (ArgumentOutOfRangeException error)
                 {

                 }
             }
             else if (userHealth.Contains("dangerous"))
             {
                 goodHealthInfo_grid.Visibility = System.Windows.Visibility.Collapsed;
                 healthInfo_grid.Visibility = System.Windows.Visibility.Visible;
             }
         }*/

        private void newPoop_button_Click(object sender, RoutedEventArgs e)
        {
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
            await SessionManagement.Logout();
            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void OnFlick(object sender, FlickGestureEventArgs e)
        {
            if (userHealth.Contains("anxious"))
            {
                // User flicked towards right
                if (e.HorizontalVelocity > 0)
                {
                    NavigationService.Navigate(new Uri("/ResultPage.xaml", UriKind.Relative));
                }
            }
        }
    }
}