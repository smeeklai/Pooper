using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor.Data
{
    public static class ShapeData
    {
        private static string[] _shapeNames = { "Separated hard lumps",
                                               "Lumpy sausage",
                                               "cracked surface sausage",
                                               "Smooth soft snake",
                                               "Soft blobs with clear cut",
                                               "Mushy and fluffy pieces",
                                               "Entirely liquid" };

        public static ReadOnlyCollection<string> ShapeNames()
        {
            return new ReadOnlyCollection<string>(_shapeNames);
        }

    }
}
