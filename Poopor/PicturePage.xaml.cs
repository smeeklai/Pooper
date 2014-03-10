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

namespace Poopor
{
    public partial class Picture_page : PhoneApplicationPage
    {

        // Variables
        private PhotoCamera cam;
        private MediaLibrary library = new MediaLibrary();
        private WriteableBitmap wb;

        public Picture_page()
        {
            InitializeComponent();
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
                String[] files = isStore.GetFileNames();
                foreach (String s in files)
                {
                    Debug.WriteLine(s);
                }
                /*using (var stream = isStore.OpenFile("poop3.jpg", FileMode.Open, FileAccess.Read))
                {
                    img.SetSource(stream);
                    Debug.WriteLine("poop3 widgt: " + img.PixelWidth + " height: " + img.PixelHeight);
                }*/
            }
            NavigationService.Navigate(new Uri("/newPoop_Info_Page.xaml", UriKind.Relative));
            /*cam.AutoFocusCompleted += cam_AutoFocusCompleted;
            if (cam != null)
            {
                try
                {
                    // Start image capture.
                    cam.FlashMode = Microsoft.Devices.FlashMode.On;
                    cam.Focus();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
            }*/
        }

        private void StoreImage(Stream stImg)
        {
            String imgName = SessionManagement.GetEmail() + "Poop" + SessionManagement.GetImageSavedCounter() + ".jpg";
            BitmapImage img = new BitmapImage();
            img.SetSource(stImg);
            wb = new WriteableBitmap(img);
            wb = wb.Resize(653, 490, WriteableBitmapExtensions.Interpolation.Bilinear);
            //Debug.WriteLine(wb.PixelWidth + " " + wb.PixelHeight);

            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveJpeg(stream, wb.PixelWidth, wb.PixelHeight, 0, 100);
                stream.Seek(0, SeekOrigin.Begin);

                // Save picture as JPEG to isolated storage.
                using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isStore.FileExists(imgName))
                        isStore.DeleteFile(imgName);
                    using (IsolatedStorageFileStream targetStream = isStore.OpenFile(imgName, FileMode.Create, FileAccess.Write))
                    {
                        // Initialize the buffer for 4KB disk pages.
                        byte[] readBuffer = new byte[4096];
                        int bytesRead = -1;

                        // Copy the image to isolated storage.
                        Debug.WriteLine("Start storing image");
                        while ((bytesRead = stream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }

                Debug.WriteLine("Save image successfully" + imgName);
            }
        }

        // Informs when full resolution photo has been taken, saves to local media library and the local folder.
        void cam_CaptureImageAvailable(object sender, Microsoft.Devices.ContentReadyEventArgs e)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                if (SessionManagement.IsLoggedIn() == false)
                {
                    BitmapImage img = new BitmapImage();
                    img.SetSource(e.ImageStream);
                    wb = new WriteableBitmap(img);
                    wb = wb.Resize(653, 490, WriteableBitmapExtensions.Interpolation.Bilinear);

                    //----------------- call PP Method here----------------------
                    AnalyzePoop();
                }
                else
                {
                    StoreImage(e.ImageStream);

                    //----------------- call PP Method here----------------------
                    AnalyzePoop();
                }
            });
            /*this.Dispatcher.BeginInvoke(delegate()
            {
                NavigationService.Navigate(new Uri("/newPoop_Info_Page.xaml", UriKind.Relative));
            });*/
        }

        private void AnalyzePoop()
        {
            SystemFunctions.SetProgressIndicatorProperties(true);
            SystemTray.ProgressIndicator.Text = "analyzing...";
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
    }
}