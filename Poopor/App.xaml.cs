using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Poopor.Resources;
using System.IO;
using SQLite;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Poopor
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        public static MobileServiceClient MobileService = new MobileServiceClient("https://poopor.azure-mobile.net/", "ArxqIFxHxlleSOeKorMFdUpZqchVVw13");

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////// ----------------- My Experiment - Start

            /* 
             1. Add sample data
             2. Create and test 3 methods
                - List<int> CountMelena  : int -> melena, all

                - ผูกสลับเสีย : bool

                - List<int> PaintCount : int -> severe, worst, all   
             */

            // - Add sample data

            /*
            Blood
            {1, "none"},
            {2, "little blood"},
            {3, "medium blood"},
            {4, "much blood"},
            {5, "a lot of blood"}
             
            P lv
            {1, "none"},
            {2, "mild"},
            {3, "moderate"},
            {4, "severe"},
            {5, "worst"}
            */
            //Debug.WriteLine("---Start---");
            //// Instantiate SQLite Function
            //SQLiteFunctions db_function_test = new SQLiteFunctions();
            
            //// Sample data  
            ////2 n n -> - **
            ////1 c -> old change to 1 *
            ////1 c -> - *
            ////3 n n n -> old = 0 (outPattern = 1,2,3 -> 0) ***
            ////1 d -> old change to 2 *
            ////1 d -> - * 
            ////1 n -> outPattern = 1 * 
            ////1 c -> old change to 1, correctPattern = 1, outPattern = 0; *
            ////1 d -> old change to 2, correctPattern = 2 * 
            ////1 n -> outPattern = 1 *
            //// 13 record

            //// New sample
            //// try correctPattern 1 -> 0
            //// c d n n n


            //// try correctPattern 2 -> 1
            //// c d (1)c (2) c d(3) n n n (2)
            //db_function_test.InsertData(new Poop_Table_SQLite()
            //{
            //    Email = "eiei_test@bank.com",
            //    Color = "Black",
            //    Shape = "Separated hard lumps",
            //    Blood_Amount = "none",
            //    Pain_Level = "worst",
            //    Having_Medicines = false,
            //    Date_Time = DateTime.Now,   // DateTime.Now, new DateTime(2014, 3, 10, 10, 10, 1)
            //    Diarrhea = true,
            //    Constipation = false,
            //    MelenaPoop = false
            //});

            ////DateTime dt = DateTime.Today.AddDays(-6);
            ////Debug.WriteLine("dt = " + dt.ToString("yyyy-MM-dd HH:mm:ss"));
            //Debug.WriteLine("---Finish Insert---");

            //String email = "eiei_test@bank.com";

            //// Test get data
            //List<Poop_Table_SQLite> Get_data = db_function_test.GetUserPoopData(email);

            //int count = 1;

            //if (Get_data == null)
            //{
            //    Debug.WriteLine("---Get_data = null---");
            //}
            //else
            //{
            //    Debug.WriteLine("---Get_data != null---");
            //    foreach (var data in Get_data)
            //    {
            //        Debug.WriteLine("Record#" + count);
            //        count++;
            //        Debug.WriteLine("Date_Time = " + data.Date_Time + " Constipation = " + data.Constipation + " Diarrhea = " + data.Diarrhea);
            //    }
            //}

            //List<int> countTest = db_function_test.CountMelenaAndPainLevel(email);
            //Debug.WriteLine("Called CountMelena -> Total = " + countTest[0] + " Melena = " + countTest[1] + " Severe = " + countTest[2] + " Worst = " + countTest[3]);
            //Debug.WriteLine("---Test START---");
            //db_function_test.IsConstipationSwapDiarrheaPattern(email);
            //Debug.WriteLine("---Test END---");
            //Debug.WriteLine("---End---");




            //////// ----------------- My Experiment - End
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //  check if database exists and create it if needed
            if (!SQLiteFunctions.FileExists("pooperDB.sqlite").Result)
            {
                using (var db = new SQLiteConnection(SQLiteFunctions.dbPath))
                {
                    db.CreateTable<UserInfo_Table_SQLite>();
                    db.CreateTable<Poop_Table_SQLite>();

                    db.CreateTable<Color_Meaning_Table_SQLite>();
                    db.CreateTable<Shape_Meaning_Table_SQLite>();
                    db.CreateTable<Short_Rec_SQLite>();
                    db.CreateTable<Long_Rec_SQLite>();
                }
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }
}