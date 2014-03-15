using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor.DataType
{
    class ColorHSL
    {
        public Double Hue { get; set; }
        public Double Saturation { get; set; }
        public Double Lightness { get; set; }

        public ColorHSL(Double hue, Double saturation, Double lightness)
        {
            // TODO: Complete member initialization
            this.Hue = hue;
            this.Saturation = saturation;
            this.Lightness = lightness;
        }
    }
}
