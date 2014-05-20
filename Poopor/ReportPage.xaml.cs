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
using System.IO;
using System.Text;
using System.Net.NetworkInformation;
using Poopor.Resources;

namespace Poopor
{
    public partial class ReportPage : PhoneApplicationPage
    {
        private string username;
        private string email;
        private string start_date;
        private string end_date;
        private byte[] byteArray;
        private StringBuilder postData;
        private Boolean isSending = false;

        public ReportPage()
        {
            InitializeComponent();
        }

        private void sendOption1_radioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void sendOption2_radioButton_Checked(object sender, RoutedEventArgs e)
        {
            anotherEmail_textBlock.Visibility = System.Windows.Visibility.Visible;
        }

        private void cancelReport_button_Click(object sender, RoutedEventArgs e)
        {
            if (isSending == false)
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = AppResources.Warning,
                    Message = "Pooper is sending your report. Are you sure you want to cancel?",
                    LeftButtonContent = "Yes",
                    RightButtonContent = "No"
                };

                messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                            break;
                    }
                };

                messageBox.Show();
            }
        }

        private void sendReport_button_Click(object sender, RoutedEventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show(AppResources.NetworkUnavailable, AppResources.NoInternetConnection, MessageBoxButton.OK);
            }
            else if (NetworkInterface.GetIsNetworkAvailable())
            {
                username = SessionManagement.GetEmail();
                start_date = ((DateTime)startDate_datePicker.Value).ToString("yyyy/MM/dd").Replace('/', '-');
                end_date = ((DateTime)endDate_datePicker.Value).ToString("yyyy/MM/dd").Replace('/', '-');
                if ((bool)sendOption2_radioButton.IsChecked)
                {
                    if (SystemFunctions.IsValidEmail(anotherEmail_textBlock.Text) == false)
                    {
                        anotherEmail_textBlock.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                        return;
                    }
                    else
                    {
                        anotherEmail_textBlock.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                        email = anotherEmail_textBlock.Text;
                    }
                }
                else
                {
                    email = SessionManagement.GetEmail();
                }
                SystemFunctions.SetProgressIndicatorProperties(true);
                SystemTray.ProgressIndicator.Text = "Sending your report...";
                postData = new StringBuilder();
                postData.Append("username=" + username + "&");
                postData.Append("start_date=" + start_date + "&");
                postData.Append("end_date=" + end_date + "&");
                postData.Append("email=" + email);

                byteArray = Encoding.UTF8.GetBytes(postData.ToString());
                System.Uri myUri = new System.Uri(("http://pooper.azurewebsites.net/index.php/report"));
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(myUri);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
                isSending = true;
                Debug.WriteLine("sending: " + isSending);
            }

        }

        private void GetRequestStreamCallback(IAsyncResult asyncResult)
        {
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;

            Stream postStream = request.EndGetRequestStream(asyncResult);

            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();


            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }

        private void GetResponseCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
            using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
            {
                string result = httpWebStreamReader.ReadToEnd();

                //For debug: show results
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                isSending = false;
                Debug.WriteLine("sending: " + isSending);
                Dispatcher.BeginInvoke(() =>
                {
                    SystemFunctions.SetProgressIndicatorProperties(false);
                    MessageBox.Show((string)data.message, (string)data.status, MessageBoxButton.OK);
                });
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (isSending == false)
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            else 
            {
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = AppResources.Warning,
                    Message = "Pooper is sending your report. Are you sure you want to cancel?",
                    LeftButtonContent = "Yes",
                    RightButtonContent = "No"
                };

                messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                            break;
                    }
                };

                messageBox.Show();
            }
        }
    }
}