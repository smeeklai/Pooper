using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    class SQLiteFunctions : DatabaseFunctions
    {

        public static string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
        private static Poop_Table_SQLite poop_table = new Poop_Table_SQLite();
        private static UserInfo_Table_SQLite userInfo_table = new UserInfo_Table_SQLite();

        public async Task<Boolean> InsertData(object data)
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

        public static async Task<bool> FileExists(string fileName)
        {
            var result = false;
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                result = true;
            }
            catch
            {
            }

            return result;
        }
    }
}
