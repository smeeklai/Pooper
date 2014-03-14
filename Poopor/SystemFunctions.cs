using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Poopor.Resources;
using Microsoft.Phone.Controls;
using System.ComponentModel;

namespace Poopor
{
    class SystemFunctions
    {
        public SystemFunctions() { 
            
        }

        public static Boolean IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                    @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

        public static Boolean IsConfirmPasswordMatched(string confirmPassword, string password)
        {
            if (confirmPassword.Length == password.Length && confirmPassword.Contains(password))
                return true;
            return false;
        }

        public static void SetProgressIndicatorProperties(bool isVisible)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        public static void ShowUnknownErrorMsgBox()
        {
            MessageBox.Show(AppResources.UnknownErrorMsg, AppResources.Warning, MessageBoxButton.OK);
        }

        public static IEnumerable<string> SortByLength(IEnumerable<string> e)
        {
            // Use LINQ to sort the array received and return a copy.
            var sorted = from s in e
                         orderby s.Length ascending
                         select s;
            return sorted;
        }

        public static async Task<Boolean> InitializeResultCriterias()
        {
            // Instantiate SQLite Function
            SQLiteFunctions db_function = new SQLiteFunctions();

            // ----- Color_Meaning_Table_SQLite : Very light brown -----                   
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Very light brown", Meaning = "Lacks in fiber" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Very light brown", Meaning = "Too much fat in the diet" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Very light brown", Meaning = "Sign of liver problems or constipation" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Very light brown", Meaning = "Sign of diarrhea symptom" });

            // ----- Color_Meaning_Table_SQLite : Medium brown -----
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Medium brown", Meaning = "Normal stool" });

            // ----- Color_Meaning_Table_SQLite : Black -----
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Eating dark colored foods" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Increased iron intake" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Too much alcohol intake" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Gastric or duodenal ulcers" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Bleeding esophageal varices" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Other types of bleeding in GI tract" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Certain medications" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Sign of abdominal pain symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Sign of vomiting symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Sign of Diarrhea symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Sign of weakness symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Black", Meaning = "Sign of dizziness symptom" });

            // ----- Color_Meaning_Table_SQLite : Maroon -----
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Intestinal parasites or infection" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Diverticulitis" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Irritable bowel syndrome(IBS)" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "GI tumors" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Polyps" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Ulcers" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Esophageal variances" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Eating red colored foods" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Too much alcohol" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Certain medications" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Sign of abdominal pain symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Sign of vomiting symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Sign of Diarrhea symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Sign of weakness symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Maroon", Meaning = "Sign of dizziness symptom" });

            // ----- Color_Meaning_Table_SQLite : Bright red -----
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Hemorrhoids" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Polyps" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Anal fissures" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Colorectal cancer" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Sign of Constipation symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Bright red", Meaning = "Sign of pain during defecation symptom" });

            // ----- Color_Meaning_Table_SQLite : Orange -----
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Bile salt production low" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Bile flow obstruction" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Lover or gall bladder disease" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Eating orange-colored foods" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Medicines" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Sign of bloating symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Sign of diarrhea symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Orange", Meaning = "Sign of abdominal discomfort symptom or pain possible " });

            // ----- Color_Meaning_Table_SQLite : Dark green -----
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Bile salt production low" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Bile flow obstruction" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Liver or gall bladder disease" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Ulcerative colitis" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Irritable bowel syndrome(IBS)" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Eating green-colored foods" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Medicines" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Sign of bloating symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Sign of diarrhea symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Dark green", Meaning = "Sign of abdominal discomfort symptom or pain possible" });

            // ----- Color_Meaning_Table_SQLite : Yellow -----
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Malabsorption of fat" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Gilbert’s Syndrome" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Eating too much yellow foods" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Sign og Parasitic infection" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Sign of Pancreatic cancer" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Sign of bloating symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Sign of Diarrhea symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Yellow", Meaning = "Sign of abdominal discomfort symptom or pain possible" });

            // ----- Color_Meaning_Table_SQLite : Gray -----
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Bile sat production low" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Bile flow obstruction" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Liver or gall bladder disease" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Sign of jaundice symptom (yellowing of skin and eyes)" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Sign of loss of appetite symptom" });
            db_function.InsertData(new Color_Meaning_Table_SQLite() { Name = "Gray", Meaning = "Sign of weakness symptom" });

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
            db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Cracked surface sausage", Meaning = "It still is ideal stool" });

            // ----- Shape_Meaning_Table_SQLite : Smooth soft snake -----
            db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Smooth soft snake", Meaning = "Most desirable state of stools" });
            db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Smooth soft snake", Meaning = "Do not exert any pressure in anal and rectal cavity while being discharged" });

            // ----- Shape_Meaning_Table_SQLite : Soft blobs with clear cut -----
            db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Soft blobs with clear cut", Meaning = "Feces move a bit fast" });
            db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Soft blobs with clear cut", Meaning = "Nutrient deficiency and dehydration can be effected from this" });
            db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Soft blobs with clear cut", Meaning = "Tending towards diarrhea" });
            db_function.InsertData(new Shape_Meaning_Table_SQLite() { Name = "Soft blobs with clear cut", Meaning = "This feces shape means food poisoning, imbalance or overgrowth of bad bacteria in the gut, bowel and/or autoimmune disease, food sensitivities, sugar dietetic candies/sweets." });

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
            db_function.InsertData(new Long_Rec_SQLite() { Name = "Maroon", L_Rec = "Avoid medications that can cause gastric irritation " });
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
            return true;
        }
    }
}
