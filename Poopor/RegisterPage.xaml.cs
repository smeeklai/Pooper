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
using System.Windows.Media;
using Poopor.Resources;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Poopor
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        private Boolean email_validation = false;
        private Boolean password_validation = false;
        private Boolean confirmPassword_validation = false;
        private Boolean firstName_validation = false;
        private Boolean lastName_validation = false;
        private Boolean weight_validation = false;
        private Boolean height_validation = false;
        private Boolean firstQuestion_validation = false;
        private Boolean secondQuestion_validation = false;
        private Boolean thridQuestion_validation = false;
        private Boolean fourtQuestion_validation = false;
        private Boolean fifthQuestion_validation = true;
        private Boolean firstQuestion_answer = false;
        private Boolean secondQuestion_answer = false;
        private Boolean thridQuestion_answer = false;
        private Boolean fourtQuestion_answer = false;
        private Boolean fifthQuestion_answer = false;
        private Boolean azureResult = false;
        private Boolean sqliteResult = false;
        private readonly DependencyProperty NetProperty = DependencyProperty.Register("NetworkAvailability",
                                         typeof(string),
                                         typeof(MainPage),
                                         new PropertyMetadata(string.Empty));

        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void signUp_button_Click(object sender, RoutedEventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show(AppResources.NetworkUnavailable, AppResources.NoInternetConnection, MessageBoxButton.OK);
            }
            else if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (CheckFirstValidation())
                {
                    if (CheckSecondLevelValidation())
                    {
                        if (await RegisterNewMember())
                        {
                            MessageBoxResult result = MessageBox.Show(AppResources.RegisterSuccessfully, AppResources.Congratulation, MessageBoxButton.OK);
                            if (result == MessageBoxResult.OK)
                            {
                                try
                                {
                                    NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                                }
                                catch (NullReferenceException b)
                                {
                                    Debug.WriteLine(b.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("These health questions are very important for us to be more accurately analyze colon cancer risk. Please answer them carefully and honestly",
                    "Few More Questions", MessageBoxButton.OK);
                        firstQuestion_answer = await ShowFirstAdditionalQuestion();
                        Debug.WriteLine(firstQuestion_answer);
                        secondQuestion_answer = await ShowSecondAdditionalQuestion();
                        Debug.WriteLine(secondQuestion_answer);
                        thridQuestion_answer = await ShowThirdAdditionalQuestion();
                        Debug.WriteLine(thridQuestion_answer);
                        fourtQuestion_answer = await ShowFourtAdditionalQuestion();
                        Debug.WriteLine(fourtQuestion_answer);
                        if (gender_picker.SelectedItem.ToString().Contains("Female"))
                        {
                            fifthQuestion_validation = false;
                            fifthQuestion_answer = await ShowFifthAdditionalQuestion();
                        }
                        Debug.WriteLine(fifthQuestion_answer);

                        if (await RegisterNewMember())
                        {
                            MessageBoxResult result = MessageBox.Show(AppResources.RegisterSuccessfully, AppResources.Congratulation, MessageBoxButton.OK);
                            if (result == MessageBoxResult.OK)
                            {
                                try
                                {
                                    NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                                }
                                catch (NullReferenceException b)
                                {
                                    Debug.WriteLine(b.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    ShowErrorMessageBox();
                }
            }
            else
            {
                SystemFunctions.ShowUnknownErrorMsgBox();
            }
        }

        private async Task<Boolean> RegisterNewMember()
        {
            if (CheckSecondLevelValidation())
            {
                if (azureResult == false)
                    azureResult = await RegisterNewMemberToAzure();
                if (azureResult == true && sqliteResult == false)
                    sqliteResult = await RegisterNewMemberToSQLite();
                if (azureResult && sqliteResult == true)
                    return true;
            }
            await CompleteSecondLevelValidation();
            return false;
        }

        private async Task<Boolean> RegisterNewMemberToAzure()
        {
            Boolean result = await new AzureFunctions().InsertData(new UserInfo_Table_Azure()
                {
                    Email = regisEmail_textBox.Text,
                    Password = regisPassword_textBox.Password,
                    FirstName = firstName_textBox.Text,
                    LastName = lastName_textBox.Text,
                    DOB = (DateTime)DOB_picker.Value,
                    Gender = gender_picker.SelectedItem.ToString(),
                    Weight = Convert.ToDouble(weight_textBox.Text),
                    Height = Convert.ToDouble(height_textBox.Text),
                    HealthInfo1 = firstQuestion_answer,
                    HealthInfo2 = secondQuestion_answer,
                    HealthInfo3 = thridQuestion_answer,
                    HealthInfo4 = fourtQuestion_answer,
                    HealthInfo5 = fifthQuestion_answer
                });

            if (result == false)
            {
                MessageBox.Show("An network error has ouccured. Please try sign up again", "Try again", MessageBoxButton.OK);
            }
            return result;
        }

        private async Task<Boolean> RegisterNewMemberToSQLite()
        {
            Boolean result = await new SQLiteFunctions().InsertData(new UserInfo_Table_SQLite()
                {
                    Email = regisEmail_textBox.Text,
                    Password = regisPassword_textBox.Password,
                    FirstName = firstName_textBox.Text,
                    LastName = lastName_textBox.Text,
                    DOB = (DateTime)DOB_picker.Value,
                    Gender = gender_picker.SelectedItem.ToString(),
                    Weight = Convert.ToDouble(weight_textBox.Text),
                    Height = Convert.ToDouble(height_textBox.Text),
                    HealthInfo1 = firstQuestion_answer,
                    HealthInfo2 = secondQuestion_answer,
                    HealthInfo3 = thridQuestion_answer,
                    HealthInfo4 = fourtQuestion_answer,
                    HealthInfo5 = fifthQuestion_answer
                });
            if (result == false)
            {
                MessageBox.Show("An system error has ouccured. Please try sign up again", "Try again", MessageBoxButton.OK);
            }
            return result;
        }

        private async Task<Boolean> CompleteSecondLevelValidation()
        {
            while (CheckSecondLevelValidation() == false)
            {
                MessageBox.Show("Please complete all the question carefully and honestly", "Some questions are still not completed", MessageBoxButton.OK);
                if (firstQuestion_validation == false)
                    firstQuestion_answer = await ShowFirstAdditionalQuestion();
                if (secondQuestion_validation == false)
                    secondQuestion_answer = await ShowSecondAdditionalQuestion();
                if (thridQuestion_validation == false)
                    thridQuestion_answer = await ShowThirdAdditionalQuestion();
                if (fourtQuestion_validation == false)
                    fourtQuestion_answer = await ShowFourtAdditionalQuestion();
                if (fifthQuestion_validation == false)
                    fifthQuestion_answer = await ShowFifthAdditionalQuestion();
            }
            return true;
        }

        private Boolean CheckFirstValidation()
        {
            if (email_validation && password_validation && confirmPassword_validation && firstName_validation && lastName_validation && weight_validation &&
                    height_validation == true)
                return true;
            else
                return false;
        }

        private Boolean CheckSecondLevelValidation()
        {
            if (firstQuestion_validation && secondQuestion_validation && thridQuestion_validation && fourtQuestion_validation && fifthQuestion_validation == true)
                return true;
            else
                return false;
        }

        private void regisEmail_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SystemFunctions.IsValidEmail(regisEmail_textBox.Text) == true)
            {
                regisEmail_textBox.BorderBrush = new SolidColorBrush(Colors.Green);
                email_validation = true;
            }
            else
            {
                regisEmail_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                email_validation = false;
            }
        }

        private void regisPassword_textBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (SystemFunctions.IsContainedEmptyText(regisPassword_textBox.Password))
            {
                regisPassword_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                password_validation = false;
            }
            else
            {
                regisPassword_textBox.BorderBrush = new SolidColorBrush(Colors.Green);
                password_validation = true;
            }
        }

        private void comfirmPassword_textBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (SystemFunctions.IsConfirmPasswordMatched(comfirmPassword_textBox.Password, regisPassword_textBox.Password))
            {
                comfirmPassword_textBox.BorderBrush = new SolidColorBrush(Colors.Green);
                confirmPassword_validation = true;
            }
            else
            {
                comfirmPassword_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                confirmPassword_validation = false;
            }
        }

        private void firstName_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SystemFunctions.IsContainedEmptyText(firstName_textBox.Text))
            {
                firstName_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                firstName_validation = false;
            }
            else
            {
                firstName_textBox.BorderBrush = new SolidColorBrush(Colors.Green);
                firstName_validation = true;
            }
        }

        private void lastName_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SystemFunctions.IsContainedEmptyText(lastName_textBox.Text))
            {
                lastName_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                lastName_validation = false;
            }
            else
            {
                lastName_textBox.BorderBrush = new SolidColorBrush(Colors.Green);
                lastName_validation = true;
            }
        }

        private void weight_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SystemFunctions.IsContainedEmptyText(weight_textBox.Text))
            {
                weight_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                weight_validation = false;
            }
            else
            {
                weight_textBox.BorderBrush = new SolidColorBrush(Colors.Green);
                weight_validation = true;
            }
        }

        private void height_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SystemFunctions.IsContainedEmptyText(height_textBox.Text))
            {
                height_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                height_validation = false;
            }
            else
            {
                height_textBox.BorderBrush = new SolidColorBrush(Colors.Green);
                height_validation = true;
            }
        }

        private void ShowErrorMessageBox()
        {
            String errorMsg = "";
            if (email_validation == false)
            {
                errorMsg += "Your email is not valid\r\n";
                regisEmail_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (password_validation == false)
            {
                errorMsg += "Your password can't be empty\r\n";
                regisPassword_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (confirmPassword_validation == false)
            {
                errorMsg += "Your Confirm password and password aren't matched\r\n";
                comfirmPassword_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (firstName_validation == false)
            {
                errorMsg += "Your first name can't be empty\r\n";
                firstName_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (lastName_validation == false)
            {
                errorMsg += "Your last name can't be empty\r\n";
                lastName_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (weight_validation == false)
            {
                errorMsg += "Your weight can't be empty\r\n";
                weight_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (height_validation == false)
            {
                errorMsg += "Your height can't be empty\r\n";
                height_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            MessageBox.Show(errorMsg, AppResources.Warning, MessageBoxButton.OK);
        }

        private async Task<Boolean> ShowFirstAdditionalQuestion()
        {

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "First Question",
                Message = "Have any of your family members(first-degree relative) been diagnosed with colon, rectum, ovarian, endometrium, or breast cancer?",
                LeftButtonContent = "Yes",
                RightButtonContent = "No"
            };
            Boolean answer = false;
            var result = await messageBox.ShowAsync();
            switch (result)
            {
                case CustomMessageBoxResult.LeftButton:
                    answer = true;
                    firstQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.RightButton:
                    answer = false;
                    firstQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.None:
                    break;
                default:
                    break;
            }
            Debug.WriteLine("Haha");
            return answer;
        }

        private async Task<Boolean> ShowSecondAdditionalQuestion()
        {

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "Second Question",
                Message = "Have you been diagnosed with inflammatory bowel disease or colorectal polyps?",
                LeftButtonContent = "Yes",
                RightButtonContent = "No"
            };
            Boolean answer = false;
            var result = await messageBox.ShowAsync();
            switch (result)
            {
                case CustomMessageBoxResult.LeftButton:
                    answer = true;
                    secondQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.RightButton:
                    answer = false;
                    secondQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.None:
                    break;
                default:
                    break;
            }
            return answer;
        }

        private async Task<Boolean> ShowThirdAdditionalQuestion()
        {

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "Third Question",
                Message = "Are you addicted to smoking or drinking alcohol?",
                LeftButtonContent = "Yes",
                RightButtonContent = "No"
            };
            Boolean answer = false;
            var result = await messageBox.ShowAsync();
            switch (result)
            {
                case CustomMessageBoxResult.LeftButton:
                    answer = true;
                    thridQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.RightButton:
                    answer = false;
                    thridQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.None:
                    break;
                default:
                    break;
            }
            return answer;
        }

        private async Task<Boolean> ShowFourtAdditionalQuestion()
        {

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "Fourt Question",
                Message = "Have you or any of your family members(first-degree relative) been diagnosed with FAP(Familial adenometous polyposis) or HNPCC(Hereditary nonpolyposis colon cancer)?",
                LeftButtonContent = "Yes",
                RightButtonContent = "No"
            };
            Boolean answer = false;
            var result = await messageBox.ShowAsync();
            switch (result)
            {
                case CustomMessageBoxResult.LeftButton:
                    answer = true;
                    fourtQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.RightButton:
                    answer = false;
                    fourtQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.None:
                    break;
                default:
                    break;
            }
            return answer;
        }

        private async Task<Boolean> ShowFifthAdditionalQuestion()
        {

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "Fifth Question",
                Message = "Have you been diagnosed with ovarian, endometrium or breast cancer?",
                LeftButtonContent = "Yes",
                RightButtonContent = "No"
            };
            Boolean answer = false;
            var result = await messageBox.ShowAsync();
            switch (result)
            {
                case CustomMessageBoxResult.LeftButton:
                    answer = true;
                    fifthQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.RightButton:
                    answer = false;
                    fifthQuestion_validation = true;
                    break;
                case CustomMessageBoxResult.None:
                    break;
                default:
                    break;
            }
            return answer;
        }
    }

    public static class ToolkitExtensions
    {
        public static Task<CustomMessageBoxResult> ShowAsync(this CustomMessageBox source)
        {

            var completion = new TaskCompletionSource<CustomMessageBoxResult>();
            // wire up the event that will be used as the result of this method
            EventHandler<DismissedEventArgs> dismissed = null;

            dismissed += (sender, args) =>
            {

                completion.SetResult(args.Result);

                // make sure we unsubscribe from this!

                source.Dismissed -= dismissed;

            };
            source.Dismissed += dismissed;
            source.Show();
            return completion.Task;
        }
    }
}