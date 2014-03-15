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
            if (!SQLiteFunctions.FileExists("db.sqlite").Result)
            {
                using (var db = new SQLiteConnection(SQLiteFunctions.dbPath))
                {
                    db.CreateTable<UserInfo_Table_SQLite>();
                    db.CreateTable<Poop_Table_SQLite>();

                    db.CreateTable<Color_Meaning_Table_SQLite>();
                    db.CreateTable<Shape_Meaning_Table_SQLite>();
                    db.CreateTable<Short_Rec_SQLite>();
                    db.CreateTable<Long_Rec_SQLite>();
                    db.CreateTable<PainLevel_Meaning_Table_SQLite>();
                    db.CreateTable<BloodAmount_Meaning_Table_SQLite>();

                    // Instantiate SQLite Function
                    SQLiteFunctions db_function = new SQLiteFunctions();

                    // ----- Color_Meaning_Table_SQLite : Very light brown -----                   
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Very light brown", Meaning = "Lacks in fiber" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Very light brown", Meaning = "Possibly take too much fat diet" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Very light brown", Meaning = "Possible sign of liver problems or constipation" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Very light brown", Meaning = "Possible sign of diarrhea" });

                    // ----- Color_Meaning_Table_SQLite : Medium brown -----
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Medium brown", Meaning = "Normal stool" });

                    // ----- Color_Meaning_Table_SQLite : Black -----
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possibly eat dark colored foods" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possibly take too much iron" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possibly drink too much alcohol" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possible sign of gastric or duodenal ulcers" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possible sign of bleeding esophageal varices" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possible other types of bleeding in GI tract" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possibly take certain medications" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possible sign of abdominal pain" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possible sign of vomiting" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possible sign of diarrhea" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possible sign of weakness" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Possible sign of dizziness" });

                    // ----- Color_Meaning_Table_SQLite : Maroon -----
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of intestinal parasites or infection" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of diverticulitis" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of irritable bowel syndrome(IBS)" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of GI tumors" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of polyps" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of ulcers" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of esophageal variances" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possibly eating red colored foods" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possibly drink too much alcohol" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possibly take certain medications" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of abdominal pain" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of vomiting" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of diarrhea" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of weakness" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Possible sign of dizziness" });

                    // ----- Color_Meaning_Table_SQLite : Bright red -----
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Possible sign of hemorrhoids" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Possible sign of polyps" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Possible sign of anal fissures" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Possible sign of colorectal cancer" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Possible sign of constipation" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Possible sign of pain during defecation" });

                    // ----- Color_Meaning_Table_SQLite : Orange -----
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Possible sign of bile salt production low" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Possible sign of bile flow obstruction" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Possible sign of liver or gall bladder disease" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Possibly eat orange-colored foods" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Possibly take certain medications" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Possible sign of bloating" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Possible sign of diarrhea" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Possible sign of abdominal discomfort symptom or pain" });

                    // ----- Color_Meaning_Table_SQLite : Dark green -----
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possible sign of bile salt production low" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possible sign of bile flow obstruction" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possible sign of liver or gall bladder disease" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possible sign of ulcerative colitis" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possible sign of irritable bowel syndrome(IBS)" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possibly eat green-colored foods" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possibly take certain medications" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possible sign of bloating" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possible sign of diarrhea" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Possible sign of abdominal discomfort symptom or pain" });

                    // ----- Color_Meaning_Table_SQLite : Yellow -----
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Possible sign of malabsorption of fat" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Possible sign of Gilbert’s Syndrome" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Possibly eat too much yellow foods" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Possible sign of parasitic infection" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Possible sign of pancreatic cancer" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Possible sign of bloating" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Possible sign of diarrhea" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Possible sign of abdominal discomfort or pain" });

                    // ----- Color_Meaning_Table_SQLite : Gray -----
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Possible sign of bile sat production low" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Possible sign of bile flow obstruction" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Possible sign of liver or gall bladder disease" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Possible sign of jaundice (yellowing of skin and eyes)" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Possible sign of loss of appetite" });
                    db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Possible sign of weakness" });

                    // ----- Shape_Meaning_Table_SQLite : Separated hard lumps -----
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Separated hard lumps", Meaning = "Lack of fiber" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Separated hard lumps", Meaning = "Sign of constipation" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Separated hard lumps", Meaning = "Insufficient water intake" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Separated hard lumps", Meaning = "Slow transit time/ Stool has spent a lot of time in colon" });

                    // ----- Shape_Meaning_Table_SQLite : Lumpy sausage -----
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Lumpy sausage", Meaning = "Sign of constipation" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Lumpy sausage", Meaning = "Stool has spent too long in colon" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Lumpy sausage", Meaning = "It is a feces like Separated hard lumps which is impacted into a single mass and lumped together by fiber components and some bacteria" });

                    // ----- Shape_Meaning_Table_SQLite : Cracked surface sausage -----
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Cracked surface sausage", Meaning = "Crack on surface means stool may be a bit dry" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Cracked surface sausage", Meaning = "Latent constipation" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Cracked surface sausage", Meaning = "Transit time is faster than feces shape Lumpy sausage between 1 and 2 weeks" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Cracked surface sausage", Meaning = "Ideal stool" });

                    // ----- Shape_Meaning_Table_SQLite : Smooth soft snake -----
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Smooth soft snake", Meaning = "Most desirable state of stools" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Smooth soft snake", Meaning = "Do not exert any pressure in anal and rectal cavity while being discharged" });

                    // ----- Shape_Meaning_Table_SQLite : Soft blobs with clear cut -----
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Soft blobs with clear cut", Meaning = "Feces move a bit fast" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Soft blobs with clear cut", Meaning = "Nutrient deficiency and dehydration can be effected from this" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Soft blobs with clear cut", Meaning = "Tending towards diarrhea" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Soft blobs with clear cut", Meaning = "Possible sign of food poisoning, imbalance or overgrowth of bad bacteria in the gut, bowel and/or autoimmune disease, food sensitivities, sugar dietetic candies/sweets" });

                    // ----- Shape_Meaning_Table_SQLite : Mushy and fluffy pieces -----
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Mushy and fluffy pieces", Meaning = "A rapid transit time" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Mushy and fluffy pieces", Meaning = "Poor absorption of nutrients" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Mushy and fluffy pieces", Meaning = "Indicate diarrhea" });

                    // ----- Shape_Meaning_Table_SQLite : Entirely liquid -----
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Entirely liquid", Meaning = "Sure sign of diarrhea" });
                    db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Entirely liquid", Meaning = "You might get some infection of some kind" });

                    // ----- Long_Rec_SQLite : Very light brown -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Very light brown", L_Rec = "Reduce fat diet" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Very light brown", L_Rec = "More fiber intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Very light brown", L_Rec = "More raw fruits and vegetables intake" });

                    // ----- Long_Rec_SQLite : Medium brown ----- *** No recommendation ***

                    // ----- Long_Rec_SQLite : Black -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Black", L_Rec = "Reduce iron intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Black", L_Rec = "Stop taking too much alcohol" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Black", L_Rec = "Avoid medications that can cause gastric irritation" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Black", L_Rec = "Consult a doctor to rule out upper GI bleeding" });

                    // ----- Long_Rec_SQLite : Maroon -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Maroon", L_Rec = "Stop taking too much alcohol" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Maroon", L_Rec = "Avoid medications that can cause gastric irritation" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Maroon", L_Rec = "Consult a doctor to rule out lower GI bleeding" });

                    // ----- Long_Rec_SQLite : Bright red -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Bright red", L_Rec = "More fiber intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Bright red", L_Rec = "More water or fluids intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Bright red", L_Rec = "Consult a doctor to rule out bleeding from cancer" });

                    // ----- Long_Rec_SQLite : Orange -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Orange", L_Rec = "More fiber intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Orange", L_Rec = "Consult a doctor to rule out liver or gall bladder disease" });

                    // ----- Long_Rec_SQLite : Dark green -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Dark green", L_Rec = "More fiber intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Dark green", L_Rec = "Consult a doctor to rule out liver, gall bladder or colonic disease" });

                    // ----- Long_Rec_SQLite : Yellow -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Yellow", L_Rec = "More fiber intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Yellow", L_Rec = "More water or fluids intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Yellow", L_Rec = "Consult a doctor to rule out pancreatic or liver disease" });

                    // ----- Long_Rec_SQLite : Gray -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Gray", L_Rec = "More nutrition and supplements" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Gray", L_Rec = "Consult a doctor for definitive treatment" });

                    // ----- Long_Rec_SQLite : Separated hard lumps -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "More water or fluids intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "More herbal teas intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "More raw fruits and vegetables intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "More cooked grains intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "Avoid meat intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "Avoid dairy intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "Avoid wheat intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "Avoid eggs intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "Avoid refined carbohydrates intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "Avoid refined sugar intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Separated hard lumps", L_Rec = "De-stress" });

                    // ----- Long_Rec_SQLite : Lumpy sausage -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "More water or fluids intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "More fiber intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "More herbal teas intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "More raw fruits and vegetables intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "More cooked grains intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "Avoid meat intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "Avoid dairy intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "Avoid wheat intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "Avoid eggs intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "Avoid refined carbohydrate intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Lumpy sausage", L_Rec = "Avoid refined sugar intake" });

                    // ----- Long_Rec_SQLite : Cracked surface sausage -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Cracked surface sausage", L_Rec = "More water or fluids intake" });

                    // ----- Long_Rec_SQLite : Smooth soft snake ----- *** No recommendation ***

                    // ----- Long_Rec_SQLite : Soft blobs with clear cut -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Soft blobs with clear cut", L_Rec = "Supplement with probiotics" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Soft blobs with clear cut", L_Rec = "More cooked grains intake" });

                    // ----- Long_Rec_SQLite : Mushy and fluffy pieces -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Mushy and fluffy pieces", L_Rec = "More fiber intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Mushy and fluffy pieces", L_Rec = "More cooked grains intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Mushy and fluffy pieces", L_Rec = "Looking for food allergies and sensitivities" });

                    // ----- Long_Rec_SQLite : Entirely liquid -----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Entirely liquid", L_Rec = "More water or fluids intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Entirely liquid", L_Rec = "More herbal teas intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Entirely liquid", L_Rec = "Eat BRAT diet (bananas, rice, applesauce, toast)" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "Entirely liquid", L_Rec = "Consult a doctor for diarrhea" });

                    // ----- Short_Rec_SQLite : Very light brown -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Very light brown", S_Rec = "Fat diet" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Very light brown", S_Rec = "Fiber" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Very light brown", S_Rec = "Fruits and vegetables" });

                    // ----- Short_Rec_SQLite : Medium brown ----- *** No recommendation ***

                    // ----- Short_Rec_SQLite : Black -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Black", S_Rec = "Consult a doctor" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Black", S_Rec = "Iron" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Black", S_Rec = "Alcohol" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Black", S_Rec = "Gastric irritation" });

                    // ----- Short_Rec_SQLite : Maroon -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Maroon", S_Rec = "Consult a doctor" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Maroon", S_Rec = "Alcohol" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Maroon", S_Rec = "Gastric irritation" });

                    // ----- Short_Rec_SQLite : Bright red -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Bright red", S_Rec = "Consult a doctor" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Bright red", S_Rec = "Fiber" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Bright red", S_Rec = "Water or fluids" });

                    // ----- Short_Rec_SQLite : Orange -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Orange", S_Rec = "Consult a doctor" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Orange", S_Rec = "Fiber" });

                    // ----- Short_Rec_SQLite : Dark green -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Dark green", S_Rec = "Consult a doctor" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Dark green", S_Rec = "Fiber" });

                    // ----- Short_Rec_SQLite : Yellow -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Yellow", S_Rec = "Consult a doctor" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Yellow", S_Rec = "Fiber" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Yellow", S_Rec = "Water or fluids" });

                    // ----- Short_Rec_SQLite : Gray -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Gray", S_Rec = "Consult a doctor" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Gray", S_Rec = "Supplements" });

                    // ----- Short_Rec_SQLite : Separated hard lumps -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Fruits and vegetables" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Meat" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "De-stress" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Refined Carbo." });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Refined sugar" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Water or fluids" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Herbal teas" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Cooked grains" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Dairy" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Wheat" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Eggs" });

                    // ----- Short_Rec_SQLite : Lumpy sausage -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Water or fluids" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Fruits and vegetables" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Meat" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Fiber" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Refined Carbo." });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Refined sugar" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Cooked grains" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Herbal teas" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Dairy" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Wheat" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Eggs" });

                    // ----- Short_Rec_SQLite : Cracked surface sausage -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Cracked surface sausage", S_Rec = "Water or fluids" });

                    // ----- Short_Rec_SQLite : Smooth soft snake ----- *** No recommendation ***

                    // ----- Short_Rec_SQLite : Soft blobs with clear cut -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Soft blobs with clear cut", S_Rec = "Supplements" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Soft blobs with clear cut", S_Rec = "Cooked grains" });

                    // ----- Short_Rec_SQLite : Mushy and fluffy pieces -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Mushy and fluffy pieces", S_Rec = "Food allergies" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Mushy and fluffy pieces", S_Rec = "Fiber" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Mushy and fluffy pieces", S_Rec = "Cooked grains" });

                    // ----- Short_Rec_SQLite : Entirely liquid -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Entirely liquid", S_Rec = "Consult a doctor" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Entirely liquid", S_Rec = "Water or fluids" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Entirely liquid", S_Rec = "Herbal teas" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "Entirely liquid", S_Rec = "BRAT diet" });

                    // ----- PainLevel_Meaning_Table_SQLite    : none -----                   
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "none", Meaning = "Well done!" });

                    // ----- PainLevel_Meaning_Table_SQLite    : mild -----                   
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "mild", Meaning = "Possibly insufficient water intake" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "mild", Meaning = "Possibly lack of fiber" });

                    // ----- PainLevel_Meaning_Table_SQLite    : moderate -----                   
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "moderate", Meaning = "Possible sign of hemorrhoids" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "moderate", Meaning = "Possible sign of anal fissure" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "moderate", Meaning = "Possible sign of proctitis" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "moderate", Meaning = "Possible sign of Crohn’s disease" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "moderate", Meaning = "Possible sign of anal cancer" });

                    // ----- PainLevel_Meaning_Table_SQLite    : severe -----                   
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "severe", Meaning = "Possible sign of hemorrhoids" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "severe", Meaning = "Possible sign of anal fissure" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "severe", Meaning = "Possible sign of proctitis" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "severe", Meaning = "Possible sign of Crohn’s disease" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "severe", Meaning = "Possible sign of anal cancer" });

                    // ----- PainLevel_Meaning_Table_SQLite    : worst -----                   
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "worst", Meaning = "Possible sign of hemorrhoids" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "worst", Meaning = "Possible sign of anal fissure" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "worst", Meaning = "Possible sign of proctitis" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "worst", Meaning = "Possible sign of Crohn’s disease" });
                    db_function.InsertData(new PainLevel_Meaning_Table_SQLite() { Name = "worst", Meaning = "Possible sign of anal cancer" });

                    // ----- BloodAmount_Meaning_Table_SQLite     : none -----                   
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "none", Meaning = "Well done!" });

                    // ----- BloodAmount_Meaning_Table_SQLite    : little blood -----                   
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "little blood", Meaning = "Possible sign of hemorrhoids" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "little blood", Meaning = "Possible sign of anal fissure" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "little blood", Meaning = "Possible sign of proctitis" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "little blood", Meaning = "Possible sign of Crohn’s disease" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "little blood", Meaning = "Possible sign of anal cancer" });

                    // ----- BloodAmount_Meaning_Table_SQLite    : medium blood -----                   
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "medium blood", Meaning = "Possible sign of hemorrhoids" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "medium blood", Meaning = "Possible sign of anal fissure" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "medium blood", Meaning = "Possible sign of proctitis" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "medium blood", Meaning = "Possible sign of Crohn’s disease" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "medium blood", Meaning = "Possible sign of anal cancer" });

                    // ----- BloodAmount_Meaning_Table_SQLite    : much blood -----                   
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "much blood", Meaning = "Possible sign of hemorrhoids" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "much blood", Meaning = "Possible sign of anal fissure" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "much blood", Meaning = "Possible sign of proctitis" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "much blood", Meaning = "Possible sign of Crohn’s disease" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "much blood", Meaning = "Possible sign of anal cancer" });

                    // ----- BloodAmount_Meaning_Table_SQLite    : a lot of blood -----                   
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "a lot of blood", Meaning = "Possible sign of hemorrhoids" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "a lot of blood", Meaning = "Possible sign of anal fissure" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "a lot of blood", Meaning = "Possible sign of proctitis" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "a lot of blood", Meaning = "Possible sign of Crohn’s disease" });
                    db_function.InsertData(new BloodAmount_Meaning_Table_SQLite() { Name = "a lot of blood", Meaning = "Possible sign of anal cancer" });

                    // ----- Long_Rec_SQLite : none (pain level, blood amount)----- no recommendation

                    // ----- Long_Rec_SQLite : mild (pain level)-----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "mild", L_Rec = "More water or fluids intake" });
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "mild", L_Rec = "More fiber intake" });

                    // ----- Long_Rec_SQLite : little blood (pain level)-----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "little blood", L_Rec = "Consult a doctor about your blood in stool" });

                    // ----- Long_Rec_SQLite : medium blood (pain level)-----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "medium blood", L_Rec = "Consult a doctor about your blood in stool" });

                    // ----- Long_Rec_SQLite : much blood (pain level)-----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "much blood", L_Rec = "Consult a doctor about your blood in stool" });

                    // ----- Long_Rec_SQLite : a lot of blood (pain level)-----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "a lot of blood", L_Rec = "Consult a doctor about your blood in stool" });

                    // ----- Long_Rec_SQLite : moderate (blood amount)-----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "moderate", L_Rec = "Consult a doctor about your painful bowel movement" });

                    // ----- Long_Rec_SQLite : severe (blood amount)-----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "severe", L_Rec = "Consult a doctor about your painful bowel movement" });

                    // ----- Long_Rec_SQLite : worst (blood amount)-----
                    db_function.InsertData(new Long_Rec_SQLite() { Name = "worst", L_Rec = "Consult a doctor about your painful bowel movement" });

                    // ----- Short_Rec_SQLite : none (pain level, blood amount) ----- no recommendation

                    // ----- Short_Rec_SQLite : mild (pain level) -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "mild", S_Rec = "Water or fluids" });
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "mild", S_Rec = "Fiber" });

                    // ----- Short_Rec_SQLite : moderate (pain level) -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "moderate", S_Rec = "Consult a doctor" });

                    // ----- Short_Rec_SQLite : severe (pain level) -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "severe", S_Rec = "Consult a doctor" });

                    // ----- Short_Rec_SQLite : worst (pain level) -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "worst", S_Rec = "Consult a doctor" });

                    // ----- Short_Rec_SQLite : little blood (blood amount) -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "little blood", S_Rec = "Consult a doctor" });

                    // ----- Short_Rec_SQLite : medium blood (blood amount) -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "medium blood", S_Rec = "Consult a doctor" });

                    // ----- Short_Rec_SQLite : much blood (blood amount) -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "much blood", S_Rec = "Consult a doctor" });

                    // ----- Short_Rec_SQLite : a lot of blood (blood amount) -----
                    db_function.InsertData(new Short_Rec_SQLite() { Name = "a lot of blood", S_Rec = "Consult a doctor" });
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