using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    class Poop_Table_SQLite
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }
        public string Color { get; set; }
        public string Shape { get; set; }
        public string Blood_Amount { get; set; }
        public string Pain_Level { get; set; }
        public bool Having_Medicines { get; set; }
        public string Poop_Picture_Name { get; set; }
        public DateTime Date_Time { get; set; }
        public bool Diarrhea { get; set; }
        public bool Constipation { get; set; }
        public bool MelenaPoop { get; set; }
    }
}
