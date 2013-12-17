using System;
using System.Drawing;
using VisualImageDiff.Helpers;


namespace VisualImageDiff.Functions.ColorFunctions
{



    public class RgbRComponent : IColorFunction
    {
        public String Name { get { return "RGB R Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            double y = (yMax - 1) * (double)color.R / 255;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class RgbGComponent : IColorFunction
    {
        public String Name { get { return "RGB G Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            double y = (yMax - 1) * (double)color.G / 255;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class RgbBComponent : IColorFunction
    {
        public String Name { get { return "RGB B Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            double y = (yMax - 1) * (double)color.B / 255;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }





    public class MsdnHsbHComponent : IColorFunction
    {
        public String Name { get { return "Msdn HSB H Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            float hue = color.GetHue();  // [0,360]
            double y = yMax * hue / 360;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class MsdnHsbSComponent : IColorFunction
    {
        public String Name { get { return "Msdn HSB S Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            float comp = color.GetSaturation();  // [0,1]
            double y = yMax * comp;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class MsdnHsbBComponent : IColorFunction
    {
        public String Name { get { return "Msdn HSB B Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            float comp = color.GetBrightness();  // [0,1]
            double y = yMax * comp;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }


    public class StaHsvVComponent : IColorFunction
    {
        public String Name { get { return "StackOv. HSV V Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            double h, s, v;
            Helper.ColorToHSV(color, out h, out s, out v);
            double y = yMax * v;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }



    public class CieXyzXComponent : IColorFunction
    {
        public String Name { get { return "CIE XYZ X Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            CIEXYZ xyz = new CIEXYZ(color);
            double y = (yMax - 1) * xyz.x;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class CieXyzYComponent : IColorFunction
    {
        public String Name { get { return "CIE XYZ Y Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            CIEXYZ xyz = new CIEXYZ(color);
            double y = (yMax - 1) * xyz.y;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class CieXyzZComponent : IColorFunction
    {
        public String Name { get { return "CIE XYZ Z Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            CIEXYZ xyz = new CIEXYZ(color);
            double y = (yMax - 1) * xyz.z/1.089;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }




    public class CieLabLComponent : IColorFunction
    {
        public String Name { get { return "Cie L*ab L Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            CIEXYZ xyz = new CIEXYZ(color);
            CIELab lab = new CIELab(xyz);
            // L belongs to [0, 100]
            double y = (yMax - 1) * lab.l / 100;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class CieLabAComponent : IColorFunction
    {
        public String Name { get { return "Cie L*ab a Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            CIEXYZ xyz = new CIEXYZ(color);
            CIELab lab = new CIELab(xyz);
            // a belongs to [-128.0, 127.0]
            double y = (yMax - 1) * (lab.a+128) / 256;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class CieLabBComponent : IColorFunction
    {
        public String Name { get { return "Cie L*ab b Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            CIEXYZ xyz = new CIEXYZ(color);
            CIELab lab = new CIELab(xyz);
            // b belongs to [-128.0, 127.0]
            double y = (yMax - 1) * (lab.b + 128) / 256;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }



    public class YuvYComponent : IColorFunction
    {
        public String Name { get { return "YUV Y Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            YUV yuv = new YUV(color);
            // we suppose Y belongs to [-1.0, 1.0]
            double y = (yMax - 1) * (yuv.y+1) / 2;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class YuvUComponent : IColorFunction
    {
        public String Name { get { return "YUV U Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            YUV yuv = new YUV(color);
            // we suppose Y belongs to [-1.0, 1.0]
            double y = (yMax - 1) * (yuv.u + 1) / 2;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class YuvVComponent : IColorFunction
    {
        public String Name { get { return "YUV V Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            YUV yuv = new YUV(color);
            // we suppose Y belongs to [-1.0, 1.0]
            double y = (yMax - 1) * (yuv.v + 1) / 2;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }




    public class AdobeRgbRComponent : IColorFunction
    {
        public String Name { get { return "Adobe RGB R Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            AdobeRGB rgb = new AdobeRGB(color);
            double y = (yMax - 1) * (rgb.r + 1) / 256;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class AdobeRgbGComponent : IColorFunction
    {
        public String Name { get { return "Adobe RGB G Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            AdobeRGB rgb = new AdobeRGB(color);
            double y = (yMax - 1) * (rgb.g + 1) / 256;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

    public class AdobeRgbBComponent : IColorFunction
    {
        public String Name { get { return "Adobe RGB B Component"; } }
        public int GetYvalue(Color color, int yMax)
        {
            AdobeRGB rgb = new AdobeRGB(color);
            double y = (yMax - 1) * (rgb.b + 1) / 256;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }

}


