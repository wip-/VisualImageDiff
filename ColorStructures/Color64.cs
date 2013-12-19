using System;
using System.Drawing;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.Helpers
{
    public struct Color64 : IColor
    {
        private UInt16 a;
        private UInt16 r;
        private UInt16 g;
        private UInt16 b;

        public double A { get { return a; } }
        public double R { get { return r; } }
        public double G { get { return g; } }
        public double B { get { return b; } }

        public double ANormalized { get { return A / 65535; } }
        public double RNormalized { get { return R / 65535; } }
        public double GNormalized { get { return G / 65535; } }
        public double BNormalized { get { return B / 65535; } }

        private Color64(int alpha, int red, int green, int blue)
        {
            a = (UInt16)alpha.Clamp0_65535();
            r = (UInt16)red.Clamp0_65535();
            g = (UInt16)green.Clamp0_65535();
            b = (UInt16)blue.Clamp0_65535();
        }

        public static Color64 FromArgb(int alpha, int red, int green, int blue)
        {
            return new Color64(alpha, red, green, blue);
        }

        public Color ToMsdnColor()
        {
            return Color.FromArgb(a / 65536, r / 65536, g / 65536, b / 65536);
        }
    }
}



