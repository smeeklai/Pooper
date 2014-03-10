using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    class Poop_Table_Azure
    {
        public String Id { get; set; }

        public String Email { get; set; }
        public String Color { get; set; }
        public String Shape { get; set; }
        public String Blood_Amount { get; set; }
        public String Pain_Level { get; set; }
        public Boolean Having_Medicines { get; set; }
        public String Poop_Picture_Name { get; set; }
        public DateTime Date_Time { get; set; }
        public Boolean Diarrhea { get; set; }
        public Boolean Constipation { get; set; }
        public Boolean MelenaPoop { get; set; }
    }
}
