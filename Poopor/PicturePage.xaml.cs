using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Poopor.Data;
using Pooper;

namespace Poopor
{
    public partial class Picture_page : PhoneApplicationPage
    {

        // Variables
        private PhotoCamera cam;
        private MediaLibrary library = new MediaLibrary();
        private WriteableBitmap poopImage;
        private Boolean havingMedicineValidation = false;
        private PoopImageProcessing pim = new PoopImageProcessing();

        public Picture_page()
        {
            InitializeComponent();
            buildApplicationBar();
        }

        //Code for initialization, capture completed, image availability events; also setting the source for the viewfinder.
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            // Check to see if the camera is available on the phone.
            if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary) == true)
            {
                // Otherwise, use standard camera on back of phone.
                cam = new Microsoft.Devices.PhotoCamera(CameraType.Primary);

                // Event is fired when the PhotoCamera object has been initialized.
                SystemFunctions.SetProgressIndicatorProperties(true);
                SystemTray.ProgressIndicator.Text = "Initializing camera...";
                cam.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(cam_Initialized);

                // Event is fired when the capture sequence is complete and an image is available.
                cam.CaptureImageAvailable += new EventHandler<Microsoft.Devices.ContentReadyEventArgs>(cam_CaptureImageAvailable);

                //Set the VideoBrush source to the camera.
                viewfinderBrush.SetSource(cam);
                viewfinderBrush.RelativeTransform =
                        new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 90 };
            }
            else
            {
                // The camera is not supported on the phone.
                this.Dispatcher.BeginInvoke(delegate()
                {
                    // Write message.
                    //txtDebug.Text = "A Camera is not available on this phone.";
                });

                // Disable UI.
                ShutterButton.IsEnabled = false;

            }
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (cam != null)
            {
                // Dispose camera to minimize power consumption and to expedite shutdown.
                cam.Dispose();

                // Release memory, ensure garbage collection.
                cam.Initialized -= cam_Initialized;
                cam.CaptureImageAvailable -= cam_CaptureImageAvailable;
                CameraButtons.ShutterKeyHalfPressed -= OnButtonHalfPress;
                CameraButtons.ShutterKeyPressed -= OnButtonFullPress;
                CameraButtons.ShutterKeyReleased -= OnButtonRelease;
                ShutterButton.Click -= ShutterButton_Click;

            }
        }

        void cam_AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            cam.CaptureImage();
        }

        // Update the UI if initialization succeeds.
        void cam_Initialized(object sender, Microsoft.Devices.CameraOperationCompletedEventArgs e)
        {
            // The event is fired when the shutter button receives a half press.
            CameraButtons.ShutterKeyHalfPressed += OnButtonHalfPress;

            // The event is fired when the shutter button receives a full press.
            CameraButtons.ShutterKeyPressed += OnButtonFullPress;

            // The event is fired when the shutter button is released.
            CameraButtons.ShutterKeyReleased += OnButtonRelease;

            ShutterButton.Click += ShutterButton_Click;
            logoutAppBar.Click += new EventHandler(logoutAppBar_Click);

            if (e.Succeeded)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    SystemFunctions.SetProgressIndicatorProperties(false);
                });
            }
        }

        private void ShutterButton_Click(object sender, RoutedEventArgs e)
        {
            using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                String[] files = isStore.GetDirectoryNames();
                foreach (String s in files)
                {
                    Debug.WriteLine(s);
                }
                //files = isStore.GetDirectoryNames();
                //foreach (String s in files)
                //{
                //    Debug.WriteLine(s);
                //}
            }
            cam.AutoFocusCompleted += cam_AutoFocusCompleted;
            if (cam != null)
            {
                try
                {
                    //Start image capture
                    cam.FlashMode = Microsoft.Devices.FlashMode.On;
                    cam.Focus();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private String StoreImage(Stream stImg)
        {
            //String imgName = SessionManagement.GetEmail() + "Poop" + SessionManagement.GetImageSavedCounter() + ".jpg";
            String imgName = System.IO.Path.GetRandomFileName() + ".jpg";
            BitmapImage img = new BitmapImage();
            img.SetSource(stImg);
            
            poopImage = new WriteableBitmap(img);
            poopImage = poopImage.Resize(653, 490, WriteableBitmapExtensions.Interpolation.Bilinear);

            using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isStore.DirectoryExists("PoopImage"))
                {
                    isStore.CreateDirectory("PoopImage");
                }
                IsolatedStorageFileStream fileStream = isStore.CreateFile(Path.Combine("PoopImage", imgName));
                poopImage.SaveJpeg(fileStream, poopImage.PixelWidth, poopImage.PixelHeight, 0, 100);
                fileStream.Close();
            }
            Debug.WriteLine("Save image successfully: " + imgName);
            return imgName;
        }

        // Informs when full resolution photo has been taken, saves to local media library and the local folder.
        void cam_CaptureImageAvailable(object sender, Microsoft.Devices.ContentReadyEventArgs e)
        {
            this.Dispatcher.BeginInvoke(async delegate()
            {
                SystemFunctions.SetProgressIndicatorProperties(true);
                if (SessionManagement.IsLoggedIn() == false)
                {
                    BitmapImage img = new BitmapImage();
                    img.SetSource(e.ImageStream);
                    poopImage = new WriteableBitmap(img);
                    poopImage = poopImage.Resize(653, 490, WriteableBitmapExtensions.Interpolation.Bilinear);

                    //----------------- call PP Method here----------------------
                    SystemTray.ProgressIndicator.Text = "Analyzing poop color...";
                    string poopColorStr = await pim.GetDominantColorTypeName(poopImage);
                    Debug.WriteLine(poopColorStr);
                    Color poopColor = await pim.GetDominantColorType(poopImage);
                    Boolean isMelena = await pim.IsMelena(poopColor);
                    Debug.WriteLine(isMelena);
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
                    "&havingMedicines=" + havingMedicines, UriKind.Relative));
                }
                else
                {
                    string poopImgName = StoreImage(e.ImageStream);
                    //----------------- call PP Method here----------------------
                    SystemTray.ProgressIndicator.Text = "Analyzing poop color...";
                    string poopColorStr = await pim.GetDominantColorTypeName(poopImage);
                    Color poopColor = await pim.GetDominantColorType(poopImage);
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
            });
            /*this.Dispatcher.BeginInvoke(delegate()
            {
                NavigationService.Navigate(new Uri("/newPoop_Info_Page.xaml", UriKind.Relative));
            });*/
        }

        private ApplicationBarMenuItem logoutAppBar = new ApplicationBarMenuItem();
        private void buildApplicationBar()
        {
            //Initialize ApplicationBar
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 0.8;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.BackgroundColor = Colors.Red;

            //Initialize ApplicationBarMenuItem
            
            logoutAppBar.Text = "Use poop sample data";

            //Add menu item
            ApplicationBar.MenuItems.Add(logoutAppBar);
        }

        private void logoutAppBar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SamplePoopPictures.xaml", UriKind.Relative));
        }

        // Provide auto-focus with a half button press using the hardware shutter button.
        private void OnButtonHalfPress(object sender, EventArgs e)
        {
            if (cam != null)
            {
                // Focus when a capture is not in progress.
                try
                {
                    cam.Focus();
                }
                catch (Exception focusError)
                {
                    // Cannot focus when a capture is in progress.
                }
            }
        }

        // Capture the image with a full button press using the hardware shutter button.
        private void OnButtonFullPress(object sender, EventArgs e)
        {
            if (cam != null)
            {
                CameraButtons.ShutterKeyReleased -= OnButtonRelease;
                cam.CaptureImage();
            }
        }

        // Cancel the focus if the half button press is released using the hardware shutter button.
        private void OnButtonRelease(object sender, EventArgs e)
        {
            if (cam != null)
            {
                cam.CancelFocus();
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