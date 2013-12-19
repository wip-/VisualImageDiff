using System;
using System.Drawing;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.Helpers
{
    // All functions inspired from http://www.codeproject.com/Articles/19045/Manipulating-colors-in-NET-Part-1
   


    public class HSL
    {
        public double h;
        public double s;
        public double l;

        public HSL(IColor rgb)
        {
            // normalizes red-green-blue values
            double nRed = rgb.RNormalized;
            double nGreen = rgb.GNormalized;
            double nBlue = rgb.BNormalized;

            double max = Math.Max(nRed, Math.Max(nGreen, nBlue));
            double min = Math.Min(nRed, Math.Min(nGreen, nBlue));

            // hue
            h = 0; // undefined
            if (max == min)
            {
                h = 0; // undefined (grayscale)
            }
            else if (max == nRed && nGreen >= nBlue)
            {
                h = 60.0 * (nGreen - nBlue) / (max - min);
            }
            else if (max == nRed && nGreen < nBlue)
            {
                h = 60.0 * (nGreen - nBlue) / (max - min) + 360.0;
            }
            else if (max == nGreen)
            {
                h = 60.0 * (nBlue - nRed) / (max - min) + 120.0;
            }
            else if (max == nBlue)
            {
                h = 60.0 * (nRed - nGreen) / (max - min) + 240.0;
            }

            // luminance
            l = (max + min) / 2.0;

            // saturation
            s = 0;
            if (l == 0 || max == min)
            {
                s = 0;
            }
            else if (0 < l && l <= 0.5)
            {
                s = (max - min) / (max + min);
            }
            else if (l > 0.5)
            {
                s = (max - min) / (2 - (max + min)); //(max-min > 0)?
            }
        }


        public Color ToRGB()
        {
            if (s == 0)
            {
                // achromatic color (gray scale)
                return Color.FromArgb(255,
                    Convert.ToByte(l * 255.0),
                    Convert.ToByte(l * 255.0),
                    Convert.ToByte(l * 255.0));
            }
            else
            {
                double q = (l < 0.5) ? (l * (1.0 + s)) : (l + s - (l * s));
                double p = (2.0 * l) - q;

                double Hk = h / 360.0;
                double[] T = new double[3];
                T[0] = Hk + (1.0 / 3.0);	// Tr
                T[1] = Hk;				// Tb
                T[2] = Hk - (1.0 / 3.0);	// Tg

                for (int i = 0; i < 3; i++)
                {
                    if (T[i] < 0) T[i] += 1.0;
                    if (T[i] > 1) T[i] -= 1.0;

                    if ((T[i] * 6) < 1)
                    {
                        T[i] = p + ((q - p) * 6.0 * T[i]);
                    }
                    else if ((T[i] * 2.0) < 1) //(1.0/6.0)<=T[i] && T[i]<0.5
                    {
                        T[i] = q;
                    }
                    else if ((T[i] * 3.0) < 2) // 0.5<=T[i] && T[i]<(2.0/3.0)
                    {
                        T[i] = p + (q - p) * ((2.0 / 3.0) - T[i]) * 6.0;
                    }
                    else T[i] = p;
                }

                return Color.FromArgb(255,
                    Convert.ToByte(T[0] * 255.0),
                    Convert.ToByte(T[1] * 255.0),
                    Convert.ToByte(T[2] * 255.0));
            }
        }
    }




    public class HSB
    {
        public double hue;
        public double sat;
        public double bri;
        public HSB (IColor rgb)
        {
            double r = rgb.RNormalized;
            double g = rgb.GNormalized;
            double b = rgb.BNormalized;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            hue = 0.0;
            if (max == r && g >= b)
            {
                if (max - min == 0)
                    hue = 0.0;
                else 
                    hue = 60 * (g - b) / (max - min);
            }
            else if (max == r && g < b)
            {
                hue = 60 * (g - b) / (max - min) + 360;
            }
            else if (max == g)
            {
                hue = 60 * (b - r) / (max - min) + 120;
            }
            else if (max == b)
            {
                hue = 60 * (r - g) / (max - min) + 240;
            }

            sat = (max == 0) ? 0.0 : (1.0 - ((double)min / (double)max));
            bri = max;
        }


        public Color ToRGB()
        {
            double r = 0;
            double g = 0;
            double b = 0;

            if (sat == 0)
            {
                r = g = b = bri;
            }
            else
            {
                // the color wheel consists of 6 sectors. Figure out which sector you're in.
                double sectorPos = hue / 60.0;
                int sectorNumber = (int)(Math.Floor(sectorPos));
                // get the fractional part of the sector
                double fractionalSector = sectorPos - sectorNumber;

                // calculate values for the three axes of the color. 
                double p = bri * (1.0 - (sat));
                double q = bri * (1.0 - (sat * fractionalSector));
                double t = bri * (1.0 - (sat * (1 - fractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        r = bri;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = bri;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = bri;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = bri;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = bri;
                        break;
                    case 5:
                        r = bri;
                        g = p;
                        b = q;
                        break;
                }
            }

            return Color.FromArgb(255,
                Convert.ToByte(r * 255.0),
                Convert.ToByte(g * 255.0),
                Convert.ToByte(b * 255.0));
        }
    }




    public class CMYK
    {
        public double c;
        public double m;
        public double y;
        public double k;

        public CMYK(IColor rgb)
        {
            double c0 = 1 - rgb.RNormalized;
            double m0 = 1 - rgb.GNormalized;
            double y0 = 1 - rgb.BNormalized;

            double min = (double)Math.Min(c0, Math.Min(m0, y0));
            if (min == 1.0)
            {
                c = 0;
                m = 0;
                y = 0;
                k = 1;
            }
            else
            {
                c = (c0 - min) / (1 - min);
                m = (m0 - min) / (1 - min);
                y = (y0 - min) / (1 - min);
                k = min;
            }
        }

        public Color ToRGB()
        {
            int red = Convert.ToInt32((1.0 - c) * (1.0 - k) * 255.0);
            int green = Convert.ToInt32((1.0 - m) * (1.0 - k) * 255.0);
            int blue = Convert.ToInt32((1.0 - y) * (1.0 - k) * 255.0);
            return Color.FromArgb(255, red, green, blue);
        }

    }



    public class YUV
    {
        public double y;
        public double u;
        public double v;

        public YUV(IColor rgb)
        {
            // normalizes red/green/blue values
            double nRed = rgb.RNormalized;
            double nGreen = rgb.GNormalized;
            double nBlue = rgb.BNormalized;

            // converts
            y = 0.299 * nRed + 0.587 * nGreen + 0.114 * nBlue;
            u = -0.1471376975169300226 * nRed - 0.2888623024830699774 * nGreen + 0.436 * nBlue;
            v = 0.615 * nRed - 0.5149857346647646220 * nGreen - 0.1000142653352353780 * nBlue;
        }

        public Color ToRGB()
        {
            return Color.FromArgb(255,
                Convert.ToInt32((y + 1.139837398373983740 * v) * 255),
                Convert.ToInt32((y - 0.3946517043589703515 * u - 0.5805986066674976801 * v) * 255),
                Convert.ToInt32((y + 2.032110091743119266 * u) * 255));
        }

    }



    public class CIEXYZ
    {
        public double x;    // belongs to [0, 0.9505]
        public double y;    // belongs to [0, 1]
        public double z;    // belongs to [0, 1.089]

        public CIEXYZ(double x, double y, double z)
        {
            this.x = x.Clamp(0, 0.9505);
            this.y = y.Clamp(0, 1);
            this.z = z.Clamp(0, 1.089);
        }

        /// Get CIE D65 (white point)
        public static readonly CIEXYZ D65 = new CIEXYZ(0.9505, 1.0, 1.0890);

        public static readonly CIEXYZ WhitePoint = new CIEXYZ(0.9505, 1.0, 1.0890);
        public static readonly CIEXYZ BlackPoint = new CIEXYZ(0, 0, 0);

        // from http://en.wikipedia.org/wiki/SRGB
        public CIEXYZ(IColor rgb)
        {
            // normalize red, green, blue values
            double rLinear = rgb.RNormalized;
            double gLinear = rgb.GNormalized;
            double bLinear = rgb.BNormalized;

            // convert to a sRGB form
            double r = (rLinear > 0.04045) ? 
                Math.Pow((rLinear + 0.055) / (1 + 0.055), 2.2) : 
                (rLinear / 12.92);
            double g = (gLinear > 0.04045) ? 
                Math.Pow((gLinear + 0.055) / (1 + 0.055), 2.2) : 
                (gLinear / 12.92);
            double b = (bLinear > 0.04045) ? 
                Math.Pow((bLinear + 0.055) / (1 + 0.055), 2.2) : 
                (bLinear / 12.92);

            // (2.2 factor is reverse gamma)

            // converts
            x = r * 0.4124 + g * 0.3576 + b * 0.1805;   // "linearized sRGB to XYZ"
            y = r * 0.2126 + g * 0.7152 + b * 0.0722;   // http://forum.doom9.org/showthread.php?p=1337213
            z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            x.Clamp(0, 0.9505);
            y.Clamp(0, 1.0);
            x.Clamp(0, 1.089);
        }

        // from http://en.wikipedia.org/wiki/SRGB
        public Color ToRGB()
        {
            double[] Clinear = new double[3];
            Clinear[0] = +x * 3.2410 - y * 1.5374 - z * 0.4986; // red
            Clinear[1] = -x * 0.9692 + y * 1.8760 + z * 0.0416; // green
            Clinear[2] = +x * 0.0556 - y * 0.2040 + z * 1.0570; // blue

            for (int i = 0; i < 3; i++)
            {
                Clinear[i] = 
                    (Clinear[i] <= 0.0031308) ? 
                    12.92 * Clinear[i] : 
                    (1 + 0.055) * Math.Pow(Clinear[i], (1.0 / 2.4)) - 0.055;
            }

            return Color.FromArgb(255,
                Convert.ToByte(Clinear[0] * 255.0),
                Convert.ToByte(Clinear[1] * 255.0),
                Convert.ToByte(Clinear[2] * 255.0));
        }


    }


    public class CIELab
    {
        public double l;    // belongs to [0, 100]
        public double a;    // belongs to [-128, 127]
        public double b;    // belongs to [-128, 127]

        /// XYZ to L*a*b* transformation function.
        private static double Fxyz(double t)
        {
            return (
                (t > 0.008856) ? 
                    Math.Pow(t, (1.0 / 3.0)) : 
                    (7.787 * t + 16.0 / 116.0));
        }

        public CIELab(CIEXYZ xyz)
        {
            l = 116.0 *  Fxyz(xyz.y / CIEXYZ.D65.y) - 16;
            a = 500.0 * (Fxyz(xyz.x / CIEXYZ.D65.x) - Fxyz(xyz.y / CIEXYZ.D65.y));
            b = 200.0 * (Fxyz(xyz.y / CIEXYZ.D65.y) - Fxyz(xyz.z / CIEXYZ.D65.z));
        }

        public CIEXYZ ToCIELab()
        {
            double theta = 6.0 / 29.0;

            double fy = (l + 16) / 116.0;
            double fx = fy + (a / 500.0);
            double fz = fy - (b / 200.0);

            return new CIEXYZ(
                (fx > theta) ? 
                    CIEXYZ.D65.x * (fx * fx * fx) : 
                    (fx - 16.0 / 116.0) * 3 * (theta * theta) * CIEXYZ.D65.x,
                (fy > theta) ? 
                    CIEXYZ.D65.y * (fy * fy * fy) :
                    (fy - 16.0 / 116.0) * 3 * (theta * theta) * CIEXYZ.D65.y,
                (fz > theta) ? 
                    CIEXYZ.D65.z * (fz * fz * fz) :
                    (fz - 16.0 / 116.0) * 3 * (theta * theta) * CIEXYZ.D65.z);
        }
    }



    public class AdobeRGB
    {
        public double r;
        public double g;
        public double b;

        public AdobeRGB(IColor color)
        {
            CIEXYZ xyz = new CIEXYZ(color);

            // http://www.adobe.com/digitalimag/pdfs/AdobeRGB1998.pdf
            r = +xyz.x * 2.04159 - xyz.y * 0.56501 - xyz.z * 0.34473;
            g = -xyz.x * 0.96924 + xyz.y * 1.87597 + xyz.z * 0.04156;
            b = +xyz.x * 0.01344 - xyz.y * 0.11836 + xyz.z * 1.01517;

            r = Math.Pow(r, (1 / 2.19921875));
            g = Math.Pow(g, (1 / 2.19921875));
            b = Math.Pow(b, (1 / 2.19921875));

            r *= 255;
            g *= 255;
            b *= 255;
        }

        public Color ToRGB()
        {
            double _r = Math.Pow(r / 255, 2.19921875);
            double _g = Math.Pow(g / 255, 2.19921875);
            double _b = Math.Pow(b / 255, 2.19921875);

            CIEXYZ xyz = new CIEXYZ(
                _r * 0.57667 + _g * 0.18556 + _b * 0.18823,
                _r * 0.29734 + _g * 0.62736 + _b * 0.07529,
                _r * 0.02703 + _g * 0.07069 + _b * 0.99134);

            return xyz.ToRGB();
        }
    }




    static class ColorConversion
    {
        public static String GetColorsString(IColor color)
        {
            String str = String.Format("[RGB] R={0:D3}, G={1:D3}, B={2:D3}\n", color.ToMsdnColor().R, color.ToMsdnColor().G, color.ToMsdnColor().B);

            str += String.Format("[HSB　MSDN] H={0:000.00}, S={1:0.000}, B={2:0.000}\n", 
                color.ToMsdnColor().GetHue(), color.ToMsdnColor().GetSaturation(), color.ToMsdnColor().GetBrightness());
            
            HSL hsl = new HSL(color);
            str += String.Format("[HSV] H={0:000.00}, S={1:0.000}, L={2:0.000}\n", hsl.h, hsl.s, hsl.l);

            HSB hsb = new HSB(color);
            str += String.Format("[HSB] H={0:000.00}, S={1:0.000}, B={2:0.000}\n", hsb.hue, hsb.sat, hsb.bri);

            CMYK cmyk = new CMYK(color);
            str += String.Format("[CMYK] C={0:0.000}, M={1:0.000}, Y={2:0.000}, K={3:0.000}\n", cmyk.c, cmyk.m, cmyk.y, cmyk.k);

            YUV yuv = new YUV(color);
            str += String.Format("[YUV] Y={0:0.000}, U={1:0.000}, V={2:0.000}\n", yuv.y, yuv.u, yuv.v);

            CIEXYZ xyz = new CIEXYZ(color);
            str += String.Format("[CIEXYZ] X={0:0.000}, Y={1:0.000}, Z={2:0.000}\n", xyz.x, xyz.y, xyz.z);

            CIELab lab = new CIELab(xyz);
            str += String.Format("[CIELAB] L={0:000.00}, a={1:000.00}, b={2:000.00}\n", lab.l, lab.a, lab.b);

            return str;
        }
    }

    


//    class CIELUV
//        TODO !!



//class ColorTemperature
//    TODO!!


//    class WaveLength
//        TODO!!!

    // class adobe RGB
    //   TODO !!!

    // class sRGB
    //   TODO !!!

    // class xyX
    //   TODO !!!

}
