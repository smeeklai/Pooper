using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pooper
{
    class PoopImageProcessing
    {
        public static Color COLOR_VERY_LIGHT_BROWN = Color.FromArgb(255, 216, 190, 112);
        public static Color COLOR_LIGHT_BROWN = Color.FromArgb(255, 108, 75, 27);
        public static Color COLOR_BLACK = Color.FromArgb(255, 20, 20, 20);
        public static Color COLOR_MORRON = Color.FromArgb(255, 75, 0, 0);
        public static Color COLOR_BRIGHT_RED = Color.FromArgb(255, 170, 0, 0);
        public static Color COLOR_ORANGE = Color.FromArgb(255, 158, 61, 2);
        public static Color COLOR_DARK_GREEN = Color.FromArgb(255, 11, 38, 4);
        public static Color COLOR_YELLOW = Color.FromArgb(255, 235, 195, 20);
        public static Color COLOR_GRAY = Color.FromArgb(255, 160, 160, 160);

        public static Dictionary<Color, String> ColorNameDictionary = new Dictionary<Color, String>()
        {
            {COLOR_VERY_LIGHT_BROWN, "Very light brown"},
            {COLOR_LIGHT_BROWN, "Medium brown"},
            {COLOR_BLACK, "Black"},
            {COLOR_MORRON, "Maroon"},
            {COLOR_BRIGHT_RED, "Bright red"},
            {COLOR_ORANGE, "Orange"},
            {COLOR_DARK_GREEN, "Dark green"},
            {COLOR_YELLOW, "Yellow"},
            {COLOR_GRAY, "Gray"},
        };

        /// <summary>
        /// Check the color is melena or not.
        /// </summary>
        /// <param name="color">The Color.</param>
        /// <returns>The boolean of melena.</returns>
        public async Task<Boolean> IsMelena(Color color)
        {
            if (color == null) {
                return false;
            }
            return (color.Equals(COLOR_BLACK) || color.Equals(COLOR_MORRON) || color.Equals(COLOR_BRIGHT_RED));
        }

        /// <summary>
        /// Gets the dominant color type name.
        /// </summary>
        /// <param name="bitmap">The WriteableBitmap.</param>
        /// <returns>The dominant color type name.</returns>
        public async Task<String> GetDominantColorTypeName(WriteableBitmap bitmap)
        {
            // Get color type name
            Color color = await GetDominantColorType(bitmap);

            return ColorNameDictionary[color];
        }

        /// <summary>
        /// Gets the dominant color type as a Color struct.
        /// </summary>
        /// <param name="bitmap">The WriteableBitmap.</param>
        /// <returns>The dominant color type as a Color struct.</returns>
        public async Task<Color> GetDominantColorType(WriteableBitmap bitmap)
        {
            // Get dominant color of image
            Color color = GetDominantColor(bitmap);

            // Generate differences table
            Dictionary<Color, float> differences = new Dictionary<Color, float>();

            // add differences key and value to table
            differences.Add(COLOR_VERY_LIGHT_BROWN, GetColorDifferences(color, COLOR_VERY_LIGHT_BROWN));
            differences.Add(COLOR_LIGHT_BROWN, GetColorDifferences(color, COLOR_LIGHT_BROWN));
            differences.Add(COLOR_BLACK, GetColorDifferences(color, COLOR_BLACK));
            differences.Add(COLOR_MORRON, GetColorDifferences(color, COLOR_MORRON));
            differences.Add(COLOR_BRIGHT_RED, GetColorDifferences(color, COLOR_BRIGHT_RED));
            differences.Add(COLOR_ORANGE, GetColorDifferences(color, COLOR_ORANGE));
            differences.Add(COLOR_DARK_GREEN, GetColorDifferences(color, COLOR_DARK_GREEN));
            differences.Add(COLOR_YELLOW, GetColorDifferences(color, COLOR_YELLOW));
            differences.Add(COLOR_GRAY, GetColorDifferences(color, COLOR_GRAY));

            // Order the dictionary of differences table by value ascending
            List<KeyValuePair<Color, float>> list = new List<KeyValuePair<Color, float>>(differences);
            list.Sort(
                delegate(KeyValuePair<Color, float> first, KeyValuePair<Color, float> next)
                {
                    return first.Value.CompareTo(next.Value);
                }
            );

            // Get the first differences value
            KeyValuePair<Color, float> type = list.First();

            // Get color
            return type.Key;
        }

        /// <summary>
        /// Gets the dominant color as a Color struct.
        /// </summary>
        /// <param name="bitmap">The WriteableBitmap.</param>
        /// <returns>The dominant color as a Color struct.</returns>
        public Color GetDominantColor(WriteableBitmap bitmap)
        {
            int xCenter = bitmap.PixelWidth / 2;
            int yCenter = bitmap.PixelHeight / 2;

            int xTopLeft = (xCenter - 50) < 0 ? 0 : xCenter - 50;
            int yTopLeft = (yCenter - 50) < 0 ? 0 : yCenter - 50;
            int xBottomRight = (xTopLeft + 100) > bitmap.PixelWidth ? bitmap.PixelWidth : xTopLeft + 100;
            int yBottomRight = (yTopLeft + 100) > bitmap.PixelHeight ? bitmap.PixelHeight : yTopLeft + 100;

            return GetDominantColor(bitmap, xTopLeft, yTopLeft, xBottomRight, yBottomRight);
        }

        /// <summary>
        /// Gets the dominant color of the pixel between the top left coordinate and bottom right coordinate as a Color struct.
        /// </summary>
        /// <param name="bitmap">The WriteableBitmap.</param>
        /// <param name="xTopLeft">The x top left coordinate of the pixel.</param>
        /// <param name="yTopLeft">The y top left coordinate of the pixel.</param>
        /// <param name="xBottomRight">The x bottom right coordinate of the pixel.</param>
        /// <param name="yBottomRight">The y bottom right coordinate of the pixel.</param>
        /// <returns>The dominant color as a Color struct.</returns>
        public static Color GetDominantColor(WriteableBitmap bitmap, int xTopLeft, int yTopLeft, int xBottomRight, int yBottomRight)
        {
            int r = 0, g = 0, b = 0;
            int total = 0;

            for (int y = yTopLeft; y < yBottomRight; y++)
            {
                for (int x = xTopLeft; x < xBottomRight; x++)
                {
                    Color color = GetPixel(bitmap, x, y);

                    r += color.R;
                    g += color.G;
                    b += color.B;

                    total++;
                }
            }

            // Calculate Average
            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb((byte)255, (byte)r, (byte)g, (byte)b);
        }

        /// <summary>
        /// Gets the color of the pixel at the x, y coordinate as a Color struct.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <param name="x">The x coordinate of the pixel.</param>
        /// <param name="y">The y coordinate of the pixel.</param>
        /// <returns>The color of the pixel at x, y as a Color struct.</returns>
        public static Color GetPixel(WriteableBitmap bitmap, int x, int y)
        {
            var c = bitmap.Pixels[y * bitmap.PixelWidth + x];
            var a = (byte)(c >> 24);

            // Prevent division by zero
            int ai = a;
            if (ai == 0)
            {
                ai = 1;
            }

            // Scale inverse alpha to use cheap integer mul bit shift
            ai = ((255 << 8) / ai);
            return Color.FromArgb(a,
                                 (byte)((((c >> 16) & 0xFF) * ai) >> 8),
                                 (byte)((((c >> 8) & 0xFF) * ai) >> 8),
                                 (byte)((((c & 0xFF) * ai) >> 8)));
        }

        /// <summary>
        /// Change brightness of image as a WriteableBitmap struct.
        /// </summary>
        /// <param name="bitmap">The WriteableBitmap.</param>
        /// <param name="brightnessValue">The brightness value.</param>
        /// <returns>The object as a WriteableBitmap struct.</returns>
        public static WriteableBitmap ChangeBrightness(WriteableBitmap bitmap, double brightnessValue)
        {
            var result = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);

            for (int i = 0; i < bitmap.Pixels.Length; i++)
            {
                // Extract color components
                var c = bitmap.Pixels[i];
                var a = (byte)(c >> 24);
                var r = (byte)(c >> 16);
                var g = (byte)(c >> 8);
                var b = (byte)(c);

                int ri = r + (int)brightnessValue;
                int gi = g + (int)brightnessValue;
                int bi = b + (int)brightnessValue;

                // Clamp to byte boundaries
                r = (byte)(ri > 255 ? 255 : (ri < 0 ? 0 : ri));
                g = (byte)(gi > 255 ? 255 : (gi < 0 ? 0 : gi));
                b = (byte)(bi > 255 ? 255 : (bi < 0 ? 0 : bi));

                result.Pixels[i] = (a << 24) | (r << 16) | (g << 8) | b;
            }

            return result;
        }

        /// <summary>
        /// Change contrast of image as a WriteableBitmap struct.
        /// </summary>
        /// <param name="bitmap">The WriteableBitmap.</param>
        /// <param name="contrastValue">The contrast value.</param>
        /// <returns>The object as a WriteableBitmap struct.</returns>
        public static WriteableBitmap ChangeContrast(WriteableBitmap bitmap, double contrastValue)
        {
            var cf = (1f + contrastValue) / 1f;
            cf *= cf;

            var cfi = (int)(cf * 32768);

            var result = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);

            for (int i = 0; i < bitmap.Pixels.Length; i++)
            {
                // Extract color components
                var c = bitmap.Pixels[i];
                var a = (byte)(c >> 24);
                var r = (byte)(c >> 16);
                var g = (byte)(c >> 8);
                var b = (byte)(c);
                int ri = r - 128;
                int gi = g - 128;
                int bi = b - 128;

                // Multiply contrast factor
                ri = (ri * cfi) >> 15;
                gi = (gi * cfi) >> 15;
                bi = (bi * cfi) >> 15;

                // Transform back to range [0, 255]
                ri = ri + 128;
                gi = gi + 128;
                bi = bi + 128;

                // Clamp to byte boundaries
                r = (byte)(ri > 255 ? 255 : (ri < 0 ? 0 : ri));
                g = (byte)(gi > 255 ? 255 : (gi < 0 ? 0 : gi));
                b = (byte)(bi > 255 ? 255 : (bi < 0 ? 0 : bi));

                result.Pixels[i] = (a << 24) | (r << 16) | (g << 8) | b;
            }

            return result;
        }

        public static bool CompareColors(Color a, Color b)
        {
            if (a.R.Equals(b.R) || a.G.Equals(b.G) || a.B.Equals(b.B))
            {
                return true;
            }
            else
            {
                return !(Math.Abs(a.R - b.R) > 9 || Math.Abs(a.G - b.G) > 9 || Math.Abs(a.B - b.B) > 9);
            }
        }

        public static float  GetColorDifferences(Color a, Color b)
        {
            int dr = Math.Abs(a.R - b.R);
            int dg = Math.Abs(a.G - b.G);
            int db = Math.Abs(a.B - b.B);

            float pdr = (float)dr / 255;
            float pdg = (float)dg / 255;
            float pdb = (float)db / 255;

            return ((pdr + pdg + pdb) / 3) * 100;
        }

    }
}
