using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Poopor.Data
{
    public class ShapeTypeToImg : IValueConverter
    {
        private static Dictionary<string, BitmapImage> ShapeNameToImg = new Dictionary<string, BitmapImage>()
        {
            {"Separated hard lumps", new BitmapImage(new Uri("/Assets/img/poopShape/stype1.png", UriKind.RelativeOrAbsolute))},
            {"Lumpy sausage", new BitmapImage(new Uri("/Assets/img/poopShape/stype2.png", UriKind.RelativeOrAbsolute))},
            {"cracked surface sausage", new BitmapImage(new Uri("/Assets/img/poopShape/stype3.png", UriKind.RelativeOrAbsolute))},
            {"Smooth soft snake", new BitmapImage(new Uri("/Assets/img/poopShape/stype4.png", UriKind.RelativeOrAbsolute))},
            {"Soft blobs with clear cut", new BitmapImage(new Uri("/Assets/img/poopShape/stype5.png", UriKind.RelativeOrAbsolute))},
            {"Mushy and fluffy pieces", new BitmapImage(new Uri("/Assets/img/poopShape/stype6.png", UriKind.RelativeOrAbsolute))},
            {"Entirely liquid", new BitmapImage(new Uri("/Assets/img/poopShape/stype7.png", UriKind.RelativeOrAbsolute))},
        };

        public static BitmapImage ConvertShapeStringToImg(string shapeName)
        {
            if (null == shapeName)
            {
                throw new ArgumentNullException("value");
            }

            BitmapImage img = null;
            if (ShapeNameToImg.TryGetValue(shapeName, out img))
            {
                return img;
            }

            return null;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string shapeName = value as string;
            if (null == shapeName)
            {
                throw new ArgumentNullException("value");
            }

            BitmapImage img = null;
            if (ShapeNameToImg.TryGetValue(shapeName, out img))
            {
                return img;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
