// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Poopor.Data
{
    /// <summary>
    /// A converter that takes a name of an accent color and returns a SolidColorBrush.
    /// </summary>
    public class AccentColorNameToBrush : IValueConverter
    {
        private static Dictionary<string, SolidColorBrush> ColorNameToBrush = new Dictionary<string, SolidColorBrush>()
        {
            { "Maroon", 0xFF4B0000.ToSolidColorBrush() },
            { "Medium brown",  0xFF6C4B1B.ToSolidColorBrush() },
            { "Very light brown",    0xFFD8BE70.ToSolidColorBrush() },
            { "Orange",    0xFF9E3D02.ToSolidColorBrush() },
            { "Yellow",   0xFFEBC314.ToSolidColorBrush() },
            { "Bright red",    0xFFAA0000.ToSolidColorBrush() },
            { "Dark green",  0xFF0B2604.ToSolidColorBrush() },
            { "Gray",    0xFFA0A0A0.ToSolidColorBrush() },
            { "Black",     0xFF141414.ToSolidColorBrush() },
        };

        public static SolidColorBrush ConvertStringToSolidColorBrush(string colorName)
        {
            if (null == colorName)
            {
                throw new ArgumentNullException("value");
            }

            SolidColorBrush brush = null;
            if (ColorNameToBrush.TryGetValue(colorName, out brush))
            {
                return brush;
            }

            return null;
        }

        /// <summary>
        /// Converts a name of an accent color to a SolidColorBrush.
        /// </summary>
        /// <param name="value">The accent color as a string.</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture</param>
        /// <returns>A SolidColorBrush representing the accent color.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "By design")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string v = value as string;
            if (null == v)
            {
                throw new ArgumentNullException("value");
            }

            SolidColorBrush brush = null;
            if (ColorNameToBrush.TryGetValue(v, out brush))
            {
                return brush;
            }

            return null;
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}