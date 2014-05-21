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
        public string Id { get; set; }

        public string Email { get; set; }
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
        public string ImageUri { get; set; }
        public string SasQueryString { get; set; }
    }
}
