using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    class SQLiteFunctions : DatabaseFunctions
    {

        public static string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "pooperDB.sqlite");
        private static Poop_Table_SQLite poop_table = new Poop_Table_SQLite();
        private static UserInfo_Table_SQLite userInfo_table = new UserInfo_Table_SQLite();
        private static Color_Meaning_Table_SQLite color_meaning_table = new Color_Meaning_Table_SQLite();
        private static Shape_Meaning_Table_SQLite shape_meaning_table = new Shape_Meaning_Table_SQLite();
        private static Short_Rec_SQLite short_rec_table = new Short_Rec_SQLite();
        private static Long_Rec_SQLite long_rec_table = new Long_Rec_SQLite();
        private static PainLevel_Meaning_Table_SQLite painLv_meaning_table = new PainLevel_Meaning_Table_SQLite();
        private static BloodAmount_Meaning_Table_SQLite bloodAmt_meaning_table = new BloodAmount_Meaning_Table_SQLite();

        public Boolean InsertData(object data)
        {
            if (IsUserInfo_Data(data))
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert((UserInfo_Table_SQLite)data);
                    });
                }
                return true;
            }
            else if (IsPoop_Data(data))
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert((Poop_Table_SQLite)data);
                    });
                }
                return true;
            }
            else if (IsColor_Meaning(data))
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert((Color_Meaning_Table_SQLite)data);
                    });
                }
                return true;
            }
            else if (IsShape_Meaning(data))
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert((Shape_Meaning_Table_SQLite)data);
                    });
                }
                return true;
            }
            else if (IsShort_Rec(data))
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert((Short_Rec_SQLite)data);
                    });
                }
                return true;
            }
            else if (IsLong_Rec(data))
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert((Long_Rec_SQLite)data);
                    });
                }
                return true;
            }
            else if (IsPainLv_Meaning(data))
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert((PainLevel_Meaning_Table_SQLite)data);
                    });
                }
                return true;
            }
            else if (IsBloodAmt_Meaning(data))
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.RunInTransaction(() =>
                    {
                        db.Insert((BloodAmount_Meaning_Table_SQLite)data);
                    });
                }
                return true;
            }
            else
                return false;
        }

        public void UpdateData(object data)
        {
            
        }

        public void DeleteData(string index)
        {

        }

        public void DeleteUserPoopData(string userEmail)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<Poop_Table_SQLite>("select * from Poop_Table_SQLite where Email='" + userEmail + "'");
                if (existing != null)
                {
                    foreach (var item in existing)
                    {
                        db.RunInTransaction(() =>
                        {
                            db.Delete(item);
                        });
                    }
                }
            }
        }

        /// <summary>
        /// In 7 days ago from today. Find pattern of constipation swap diarrhea
        /// Return : true or false
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>

        public bool IsConstipationSwapDiarrheaPattern(string userEmail)
        {
            int correctPatternCount = 0;
            bool isNowConstipation = false;
            bool isNowDiarrhea = false;
            int oldStatus = 0; // 0 = default, 1 = Constipation, 2 = Diarrhea
            int currentStatus = 0; // 0 = default, 1 = Constipation, 2 = Diarrhea
            int countOutOfPattern = 0;

            int countRecord = 1;
            using (var db = new SQLiteConnection(dbPath))
            {
                DateTime StartDate = DateTime.Today.AddDays(-6); // We count today is the last date, so just look back only 6 day from today
                DateTime EndDate = DateTime.Today;
                var existing = db.Query<Poop_Table_SQLite>("select * from Poop_Table_SQLite where Email='" + userEmail + "' order by date_time desc");


                if (existing != null)
                {
                    foreach (var data in existing)
                    {
                        DateTime dataDateTime = DateTime.Parse(data.Date_Time.ToString("yyyy-MM-dd"));

                        if (dataDateTime >= StartDate && dataDateTime <= EndDate)
                        {
                            if (data.Constipation)
                            {
                                isNowConstipation = true;
                                isNowDiarrhea = false;
                            }
                            if (data.Diarrhea)
                            {
                                isNowConstipation = false;
                                isNowDiarrhea = true;
                            }
                            if (data.Constipation == false && data.Diarrhea == false)
                            {
                                isNowConstipation = false;
                                isNowDiarrhea = false;
                            }

                            if (oldStatus != 0)
                            {// If there is abnormal poop
                                if (isNowConstipation) currentStatus = 1;
                                else if (isNowDiarrhea) currentStatus = 2;
                                else currentStatus = 0;

                                if (currentStatus != 0)
                                {// Check pattern for check that is it swap between costipation and diarrhea or not
                                    countOutOfPattern = 0;
                                    if (oldStatus != currentStatus)
                                    {// If it swap, then increase correct pattern and change the oldStatus
                                        correctPatternCount++;
                                        oldStatus = currentStatus;
                                    }
                                }
                                else
                                {// check to decrease the range of the sign of this pattern
                                    countOutOfPattern++;
                                    if (countOutOfPattern == 4)
                                    {
                                        countOutOfPattern = 0;
                                        oldStatus = 0;
                                        correctPatternCount--;
                                        if (correctPatternCount < 0) correctPatternCount = 0;
                                    }
                                }


                            }
                            else
                            {// First time for abnormal poop
                                if (isNowConstipation) oldStatus = 1;
                                if (isNowDiarrhea) oldStatus = 2;
                            }


                        }
                        else break;

                        //Debug.WriteLine("---IN Test Record#" + countRecord + ": DateTime = " + data.Date_Time.ToString("yyyy-MM-dd HH:mm:ss"));
                        //Debug.WriteLine("--isNowConstipation = " + isNowConstipation + " isNowDiarrhea = " + isNowDiarrhea);
                        //Debug.WriteLine("-oldStatus = " + oldStatus + " currentStatus = " + currentStatus + " correctPatternCount = " + correctPatternCount
                        //    + " countOutOfPattern = " + countOutOfPattern);
                        //countRecord++;
                    }
                    return (correctPatternCount > 1);
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// In 7 days ago from today. Find total bowel movement times, number of melena, number of pain level severe, and number of pain level worst 
        /// Return : List of int that index 0 = total bowel movement times, index 1 = number of melena, index 2 = number of pain level severe, index 3 = number of pain level worst
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>

        public List<int> CountMelenaAndPainLevel(string userEmail)
        {
            List<int> countMelenaAndPainLevelList = new List<int>();
            using (var db = new SQLiteConnection(dbPath))
            {
                DateTime StartDate = DateTime.Today.AddDays(-6); // We count today is the last date, so just look back only 6 day from today
                DateTime EndDate = DateTime.Today;
                var existing = db.Query<Poop_Table_SQLite>("select * from Poop_Table_SQLite where Email='" + userEmail + "' order by date_time desc");

                int totalBowelMovementTimesIn_7_Days = 0;
                int melenaCount = 0;
                int severePainCount = 0;
                int worstPainCount = 0;
                if (existing != null)
                {
                    foreach (var data in existing)
                    {
                        DateTime dataDateTime = DateTime.Parse(data.Date_Time.ToString("yyyy-MM-dd"));

                        if (dataDateTime >= StartDate && dataDateTime <= EndDate)
                        {
                            totalBowelMovementTimesIn_7_Days++;
                            if (data.MelenaPoop == true) melenaCount++;
                            if ((data.Pain_Level).Equals("severe", StringComparison.Ordinal)) severePainCount++;
                            if ((data.Pain_Level).Equals("worst", StringComparison.Ordinal)) worstPainCount++;
                        }
                        else break;
                    }
                    countMelenaAndPainLevelList.Add(totalBowelMovementTimesIn_7_Days);
                    countMelenaAndPainLevelList.Add(melenaCount);
                    countMelenaAndPainLevelList.Add(severePainCount);
                    countMelenaAndPainLevelList.Add(worstPainCount);
                    return countMelenaAndPainLevelList;
                }
                else
                    return null;
            }
        }
            
        public List<string> GetColorMeaning(string color)
        {
            List<string> list = new List<string>();
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<Color_Meaning_Table_SQLite>("select * from Color_Meaning_Table_SQLite where Name='" + color + "'");
                if (existing != null)
                {
                    foreach (var data in existing)
                    {
                        list.Add(data.Meaning);
                    }
                    return list;
                }
                else
                    return null;
            }
        }

        public List<String> GetShapeMeaning(string shape)
        {
            List<string> list = new List<string>();
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<Shape_Meaning_Table_SQLite>("select * from Shape_Meaning_Table_SQLite where Name='" + shape + "'");
                if (existing != null)
                {
                    foreach (var data in existing)
                    {
                        list.Add(data.Meaning);
                    }
                    return list;
                }
                else
                    return null;
            }
        }

        public List<String> GetPainLvMeaning(string painLv)
        {
            List<string> list = new List<string>();
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<PainLevel_Meaning_Table_SQLite>("select * from PainLevel_Meaning_Table_SQLite where Name='" + painLv + "'");
                if (existing != null)
                {
                    foreach (var data in existing)
                    {
                        list.Add(data.Meaning);
                    }
                    return list;
                }
                else
                    return null;
            }
        }

        public List<String> GetBloodAmtMeaning(string bloodAmt)
        {
            List<string> list = new List<string>();
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<BloodAmount_Meaning_Table_SQLite>("select * from BloodAmount_Meaning_Table_SQLite where Name='" + bloodAmt + "'");
                if (existing != null)
                {
                    foreach (var data in existing)
                    {
                        list.Add(data.Meaning);
                    }
                    return list;
                }
                else
                    return null;
            }
        }

        public List<String> GetShortRec(string color, string shape, string painLv, string bloodAmt)
        {
            List<string> list = new List<string>();
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<Short_Rec_SQLite>("select distinct S_Rec from Short_Rec_SQLite where Name='" + color + "' or Name ='" + shape +
                    "' or Name ='" + painLv + "' or Name ='"+ bloodAmt + "' limit 6");
                if (existing != null)
                {
                    foreach (var data in existing)
                    {
                        list.Add(data.S_Rec);
                    }
                    return list;
                }
                else
                    return null;
            }
        }

        public List<String> GetLongRec(string color, string shape, string painLv, string bloodAmt)
        {
            List<string> list = new List<string>();
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<Long_Rec_SQLite>("select distinct L_Rec from Long_Rec_SQLite where Name='" + color + "' or Name ='" + shape +
                    "' or Name ='" + painLv + "' or Name ='" + bloodAmt + "'");
                if (existing != null)
                {
                    foreach (var data in existing)
                    {
                        list.Add(data.L_Rec);
                    }
                    return list;
                }
                else
                    return null;
            }
        }

        public UserInfo_Table_SQLite GetUserInfo(string userEmail)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<UserInfo_Table_SQLite>("select * from UserInfo_Table_SQLite where Email='" + userEmail + "'").FirstOrDefault();
                if (existing != null)
                    return existing;
                else
                    return null;
            }
        }

        public List<Poop_Table_SQLite> GetUserPoopData(string userEmail)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<Poop_Table_SQLite>("select * from Poop_Table_SQLite where Email='" + userEmail + "'");
                if (existing != null)
                    return existing;
                else
                    return null;
            }
        }

        public List<UserInfo_Table_SQLite> GetUserInfoByQuery(string query)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<UserInfo_Table_SQLite>(query);
                if (existing != null)
                    return existing;
                else
                    return null;
            }
        }

        public List<Poop_Table_SQLite> GetUserPoopDataByQuery(string query)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                var existing = db.Query<Poop_Table_SQLite>(query);
                if (existing != null)
                    return existing;
                else
                    return null;
            }
        }

        private Boolean IsUserInfo_Data(object data)
        {
            if (userInfo_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }

        private Boolean IsPoop_Data(object data)
        {
            if (poop_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }

        private Boolean IsColor_Meaning(object data)
        {
            if (color_meaning_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }

        private Boolean IsShape_Meaning(object data)
        {
            if (shape_meaning_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }


        private Boolean IsPainLv_Meaning(object data)
        {
            if (painLv_meaning_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }


        private Boolean IsBloodAmt_Meaning(object data)
        {
            if (bloodAmt_meaning_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }

        private Boolean IsShort_Rec(object data)
        {
            if (short_rec_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }

        private Boolean IsLong_Rec(object data)
        {
            if (long_rec_table.GetType().Equals(data.GetType()))
                return true;
            return false;
        }

        public static async Task<bool> FileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine("SQLite doesn't exist");
            }
            return false;
        }

        public static Boolean IsResultCriteriaInitialized()
        {
            using (var db = new SQLiteConnection(SQLiteFunctions.dbPath))
            {
                var existing = db.Query<UserInfo_Table_SQLite>("select * from Color_Meaning_Table_SQLite");
                if (existing.Count != 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
