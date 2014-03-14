using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    class Shape_Meaning_Table_SQLite
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }

        public String Name { get; set; }
        public String Meaning { get; set; }
    }
}
