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

namespace Poopor
{
    public partial class Picture_page : PhoneApplicationPage
    {

        // Variables
        private int savedCounter = 0;
        PhotoCamera cam;
        MediaLibrary library = new MediaLibrary();
        Popup p = new Popup();

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
                cam.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(cam_Initialized);

                // Event is fired when the capture sequence is complete.
                cam.CaptureCompleted += new EventHandler<CameraOperationCompletedEventArgs>(cam_CaptureCompleted);

                // Event is fired when the capture sequence is complete and an image is available.
                cam.CaptureImageAvailable += new EventHandler<Microsoft.Devices.ContentReadyEventArgs>(cam_CaptureImageAvailable);

                // The event is fired when the shutter button receives a half press.
                CameraButtons.ShutterKeyHalfPressed += OnButtonHalfPress;

                // The event is fired when the shutter button receives a full press.
                CameraButtons.ShutterKeyPressed += OnButtonFullPress;

                // The event is fired when the shutter button is released.
                CameraButtons.ShutterKeyReleased += OnButtonRelease;

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
                cam.CaptureCompleted -= cam_CaptureCompleted;
                cam.CaptureImageAvailable -= cam_CaptureImageAvailable;
                CameraButtons.ShutterKeyHalfPressed -= OnButtonHalfPress;
                CameraButtons.ShutterKeyPressed -= OnButtonFullPress;
                CameraButtons.ShutterKeyReleased -= OnButtonRelease;

            }
        }

        void cam_AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            cam.CaptureImage();
        }

        // Update the UI if initialization succeeds.
        void cam_Initialized(object sender, Microsoft.Devices.CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    // Write message.
                    //txtDebug.Text = "Camera initialized.";
                });
            }
        }

        private void ShutterButton_Click(object sender, RoutedEventArgs e)
        {
            /*using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                String[] files = isStore.GetFileNames();
                foreach (String s in files)
                {
                    Debug.WriteLine(s);
                }
                //Debug.WriteLine(isStore.g);
            }*/
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
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        // Cannot capture an image until the previous capture has completed.
                        //txtDebug.Text = ex.Message;
                    });
                }
            }*/
        }

        void cam_CaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            // Increments the savedCounter variable used for generating JPEG file names.
            savedCounter++;
        }

        // Informs when full resolution photo has been taken, saves to local media library and the local folder.
        void cam_CaptureImageAvailable(object sender, Microsoft.Devices.ContentReadyEventArgs e)
        {
            String imgName = "poop" + savedCounter + ".jpg";
            try
            {
                // Save picture as JPEG to isolated storage.
                using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    
                    using (IsolatedStorageFileStream targetStream = isStore.OpenFile(imgName, FileMode.Create, FileAccess.Write))
                    {
                        // Initialize the buffer for 4KB disk pages.
                        byte[] readBuffer = new byte[4096];
                        int bytesRead = -1;

                        // Copy the image to isolated storage. 
                        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }

                Debug.WriteLine("Save image successfully" + imgName);
            }
            finally
            {
                // Close image stream
                e.ImageStream.Close();
            }

            /*this.Dispatcher.BeginInvoke(delegate()
            {
                NavigationService.Navigate(new Uri("/newPoop_Info_Page.xaml", UriKind.Relative));
            });*/
        }

        // Provide auto-focus with a half button press using the hardware shutter button.
        private void OnButtonHalfPress(object sender, EventArgs e)
        {
            if (cam != null)
            {
                // Focus when a capture is not in progress.
                try
                {
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        //txtDebug.Text = "Half Button Press: Auto Focus";
                    });

                    cam.Focus();
                }
                catch (Exception focusError)
                {
                    // Cannot focus when a capture is in progress.
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        //txtDebug.Text = focusError.Message;
                    });
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