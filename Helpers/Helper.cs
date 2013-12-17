using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace VisualImageDiff
{
    public static class Helper
    {
        static public int GetComponentsNumber(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    return 1;

                case PixelFormat.Format24bppRgb:
                    return 3;

                case PixelFormat.Format32bppArgb:
                    return 4;

                default:
                    Debug.Assert(false);
                    return 0;
            }
        }


        static public Color GetColorDiff(Color left, Color right)
        {
            int a = Math.Abs(left.A - right.A);
            int r = Math.Abs(left.R - right.R);
            int g = Math.Abs(left.G - right.G);
            int b = Math.Abs(left.B - right.B);

            return Color.FromArgb(
                Convert.ToByte(a.Clamp0_255()),
                Convert.ToByte(r.Clamp0_255()),
                Convert.ToByte(g.Clamp0_255()),
                Convert.ToByte(b.Clamp0_255()));
        }

        public static int Clamp0_255(this int value)
        {
            return value.Clamp(0, 255);
        }

        public static float Clamp0_255(this float value)
        {
            return value.Clamp(0, 255);
        }

        public static double Clamp0_255(this double value)
        {
            return value.Clamp(0, 255);

        }
        //http://stackoverflow.com/a/2683487/758666
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }



        //http://stackoverflow.com/a/1626175/758666
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }


        //http://stackoverflow.com/a/1626175/758666
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }


        // http://en.wikipedia.org/wiki/Luminance_(relative)
        // weights each R, G, B component. Sum of params is 1.00
        // so result is between [0, 255]
        // alternative formulas http://stackoverflow.com/a/596243/758666
        public static double GetRelativeLuminance(Color c)
        {
            //return 
            //    0.2126 * (double)c.R +
            //    0.7152 * (double)c.G +
            //    0.0722 * (double)c.B;

            //return
            //    0.299 * (double)c.R +
            //    0.587 * (double)c.G +
            //    0.114 * (double)c.B;

            return Math.Sqrt(
                0.241 * (double)c.R * (double)c.R +
                0.691 * (double)c.G * (double)c.G +
                0.068 * (double)c.B * (double)c.B);

        }

















    }
}
