using System.Drawing;

namespace VisualImageDiff.ColorStructures
{
    public struct Color32 : IColor
    {
        private Color msdnColor;

        private Color32(int alpha, int red, int green, int blue)
        {
            msdnColor = Color.FromArgb(alpha, red, green, blue);
        }

        public double A { get { return msdnColor.A; } }
        public double R { get { return msdnColor.R; } }
        public double G { get { return msdnColor.G; } }
        public double B { get { return msdnColor.B; } }

        public double ANormalized { get { return A / 255; } }
        public double RNormalized { get { return R / 255; } }
        public double GNormalized { get { return G / 255; } }
        public double BNormalized { get { return B / 255; } }

        //public Byte A { get { return msdnColor.A; } }
        //public Byte R { get { return msdnColor.R; } }
        //public Byte G { get { return msdnColor.G; } }
        //public Byte B { get { return msdnColor.B; } }

        public static Color32 FromArgb(int alpha, int red, int green, int blue)
        {
            return new Color32(alpha, red, green, blue);
        }

        //public static implicit operator Color(Color32 color32)
        //{
        //    return color32.msdnColor;
        //}
        public static implicit operator Color32(Color color)
        {
            return Color32.FromArgb(color.A, color.R, color.G, color.B);
        }
        public Color ToMsdnColor()
        {
            return msdnColor;
        }
    }

}
