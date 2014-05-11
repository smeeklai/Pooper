    using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    class UserInfo_Table_Azure
    {
        public string Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public Boolean HealthInfo1 { get; set; }
        public Boolean HealthInfo2 { get; set; }
        public Boolean HealthInfo3 { get; set; }
        public Boolean HealthInfo4 { get; set; }
        public Boolean HealthInfo5 { get; set; }
    }
}
