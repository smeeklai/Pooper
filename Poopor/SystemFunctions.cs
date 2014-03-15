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
using System.Xml.Serialization;

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

        public static List<ResultAndRecommendationDictionary> SerializeUserResultAndRecommendationData(Dictionary<string, List<string>> userResultAndRecommendationData)
        {
            List<ResultAndRecommendationDictionary> tempDataItems = new List<ResultAndRecommendationDictionary>(userResultAndRecommendationData.Count);
            foreach (var item in userResultAndRecommendationData)
            {
                tempDataItems.Add(new ResultAndRecommendationDictionary(item.Key, item.Value));
            }

            Debug.WriteLine("Serialized data finished");
            return tempDataItems;
        }

        public static Dictionary<string, List<string>> DeserializeUserResultAndRecommendationData(List<ResultAndRecommendationDictionary> serializedUserResultAndRecommendationData)
        {
            Dictionary<string, List<string>> resultDic = new Dictionary<string, List<string>>();

            foreach (ResultAndRecommendationDictionary item in serializedUserResultAndRecommendationData)
            {
                resultDic.Add(item.Key, item.Value);
            }
            return resultDic;
        }

        public static void InitializeResultCriterias()
        {
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
            db_function.InsertData(new Short_Rec_SQLite() { Name = "Separated hard lumps", S_Rec = "Refined Carbo" });
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
            db_function.InsertData(new Short_Rec_SQLite() { Name = "Lumpy sausage", S_Rec = "Refined Carbo" });
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
            //return true;
        }
    }
}
