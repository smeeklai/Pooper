using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    interface DatabaseFunctions
    {
        Task<Boolean> InsertData(object data);
        void UpdateData(object data);
        void DeleteData(string index);
        void getData();
    }
}
