using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using Poopor.Data;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Pooper;
using System.Threading.Tasks;
using System.Windows.Resources;

namespace Poopor
{
    public partial class SamplePoopPictures : PhoneApplicationPage
    {
        private PoopImageData previousSelectedPoopImage = null;
        private string currentPoopImageUri;
        private PoopImageProcessing pim = new PoopImageProcessing();
        private Boolean havingMedicineValidation = false;
        private string poopImgName;

        public SamplePoopPictures()
        {
            InitializeComponent();
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PoopImageData currentSelectedPoopImage = ((FrameworkElement)sender).DataContext as PoopImageData;
            currentPoopImageUri = currentSelectedPoopImage.ImgUrl;
            poopImgName = currentSelectedPoopImage.PoopImgName;
            if (currentSelectedPoopImage != null)
            {
                if (currentSelectedPoopImage.Equals(previousSelectedPoopImage))
                {
                    currentSelectedPoopImage.IsSelected = false;
                    previousSelectedPoopImage = null;
                    currentPoopImageUri = null;
                    poopImgName = null;
                }
                else
                {
                    currentSelectedPoopImage.IsSelected = true;
                    if (previousSelectedPoopImage != null)
                    {
                        previousSelectedPoopImage.IsSelected = false;
                    }

                }
            }
            previousSelectedPoopImage = currentSelectedPoopImage;
        }

        private async void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if (currentPoopImageUri == null)
            {
                MessageBox.Show("You must select one sample poop picture", "Warning", MessageBoxButton.OK);
            }
            else
            {
                SystemFunctions.SetProgressIndicatorProperties(true);
                SystemTray.ProgressIndicator.Text = "Analyzing poop color...";
                Uri uri = new Uri(currentPoopImageUri, UriKind.Relative);
                BitmapImage poopImg = new BitmapImage(uri);
                poopImg.CreateOptions = BitmapCreateOptions.None;
                poopImg.ImageOpened += poopImg_ImageOpened;
            }
        }

        async void poopImg_ImageOpened(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wbPoopImg = new WriteableBitmap((BitmapImage)sender);
            if (wbPoopImg != null)
            {
                string poopColorStr = await pim.GetDominantColorTypeName(wbPoopImg);
                Color poopColor = await pim.GetDominantColorType(wbPoopImg);
                Boolean isMelena = await pim.IsMelena(poopColor);
                SystemFunctions.SetProgressIndicatorProperties(false);
                Boolean havingMedicines = false;
                if (isMelena)
                {
                    while (!havingMedicineValidation)
                    {
                        havingMedicines = await IsUserHavingMedicine();
                    }
                }
                NavigationService.Navigate(new Uri("/newPoop_Info_Page.xaml?poopColor=" + poopColorStr + "&melenaResult=" + isMelena +
                    "&havingMedicines=" + havingMedicines + "&poopImgName=" + poopImgName, UriKind.Relative));
            }
        }

        private async Task<Boolean> IsUserHavingMedicine()
        {

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "Additional Question!",
                Message = "Are you having some medicines that have an effect to your poop color?",
                LeftButtonContent = "Yes",
                RightButtonContent = "No"
            };
            Boolean answer = false;
            var result = await messageBox.ShowAsync();
            switch (result)
            {
                case CustomMessageBoxResult.LeftButton:
                    answer = true;
                    havingMedicineValidation = true;
                    break;
                case CustomMessageBoxResult.RightButton:
                    answer = false;
                    havingMedicineValidation = true;
                    break;
                case CustomMessageBoxResult.None:
                    break;
                default:
                    break;
            }
            return answer;
        }
    }
}