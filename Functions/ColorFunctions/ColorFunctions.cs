using System;
using VisualImageDiff.ColorStructures;
using VisualImageDiff.Helpers;


namespace VisualImageDiff.Functions.ColorFunctions
{



    public class RgbRComponent : BaseColorFunction
    {
        public override String Name { get { return "RGB R Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 255; } }
        public override double GetYvalue(IColor color){ return color.R; }
    }

    public class RgbGComponent : BaseColorFunction
    {
        public override String Name { get { return "RGB G Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 255; } }
        public override double GetYvalue(IColor color) { return color.G; }
    }

    public class RgbBComponent : BaseColorFunction
    {
        public override String Name { get { return "RGB B Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 255; } }
        public override double GetYvalue(IColor color) { return color.B; }
    }




    public class MsdnHsbHComponent : BaseColorFunction
    {
        public override String Name { get { return "Msdn HSB H Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 360; } }
        public override double GetYvalue(IColor color) { return color.ToMsdnColor().GetHue(); }
    }

    public class MsdnHsbSComponent : BaseColorFunction
    {
        public override String Name { get { return "Msdn HSB S Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 1; } }
        public override double GetYvalue(IColor color) { return color.ToMsdnColor().GetSaturation(); }
    }

    public class MsdnHsbBComponent : BaseColorFunction
    {
        public override String Name { get { return "Msdn HSB B Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 1; } }
        public override double GetYvalue(IColor color) { return color.ToMsdnColor().GetBrightness(); }
    }



    public class StaHsvVComponent : BaseColorFunction
    {
        public override String Name { get { return "StackOv. HSV V Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 1; } }
        public override double GetYvalue(IColor color)
        {
            double h, s, v;
            Helper.ColorToHSV(color, out h, out s, out v);
            return v;
        }
    }


    public class CieXyzXComponent : BaseColorFunction
    {
        public override String Name { get { return "CIE XYZ X Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 1; } }
        public override double GetYvalue(IColor color) { return new CIEXYZ(color).x; }
    }

    public class CieXyzYComponent : BaseColorFunction
    {
        public override String Name { get { return "CIE XYZ Y Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 1; } }
        public override double GetYvalue(IColor color) { return new CIEXYZ(color).y; }
    }

    public class CieXyzZComponent : BaseColorFunction
    {
        public override String Name { get { return "CIE XYZ Z Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 1; } }
        public override double GetYvalue(IColor color) { return new CIEXYZ(color).z; }
    }



    public class CieLabLComponent : BaseColorFunction
    {
        public override String Name { get { return "Cie L*ab L Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 100; } }
        public override double GetYvalue(IColor color) { return new CIELab(new CIEXYZ(color)).l; }
    }

    public class CieLabAComponent : BaseColorFunction
    {
        public override String Name { get { return "Cie L*ab a Component"; } }
        public override double yInputMin { get { return -128; } }
        public override double yInputMax { get { return  128; } }
        public override double GetYvalue(IColor color) { return new CIELab(new CIEXYZ(color)).a; }
    }

    public class CieLabBComponent : BaseColorFunction
    {
        public override String Name { get { return "Cie L*ab b Component"; } }
        public override double yInputMin { get { return -128; } }
        public override double yInputMax { get { return  128; } }
        public override double GetYvalue(IColor color) { return new CIELab(new CIEXYZ(color)).b; }
    }




    public class YuvYComponent : BaseColorFunction
    {
        public override String Name { get { return "YUV Y Component"; } }
        public override double yInputMin { get { return -1; } }
        public override double yInputMax { get { return  1; } }
        public override double GetYvalue(IColor color) { return new YUV(color).y; }
    }

    public class YuvUComponent : BaseColorFunction
    {
        public override String Name { get { return "YUV U Component"; } }
        public override double yInputMin { get { return -1; } }
        public override double yInputMax { get { return  1; } }
        public override double GetYvalue(IColor color) { return new YUV(color).u; }
    }

    public class YuvVComponent : BaseColorFunction
    {
        public override String Name { get { return "YUV V Component"; } }
        public override double yInputMin { get { return -1; } }
        public override double yInputMax { get { return  1; } }
        public override double GetYvalue(IColor color) { return new YUV(color).v; }
    }



    public class AdobeRgbRComponent : BaseColorFunction
    {
        public override String Name { get { return "Adobe RGB R Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 255; } }
        public override double GetYvalue(IColor color) { return new AdobeRGB(color).r; }
    }

    public class AdobeRgbGComponent : BaseColorFunction
    {
        public override String Name { get { return "Adobe RGB G Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 255; } }
        public override double GetYvalue(IColor color) { return new AdobeRGB(color).g; }
    }

    public class AdobeRgbBComponent : BaseColorFunction
    {
        public override String Name { get { return "Adobe RGB B Component"; } }
        public override double yInputMin { get { return 0; } }
        public override double yInputMax { get { return 255; } }
        public override double GetYvalue(IColor color) { return new AdobeRGB(color).b; }
    }

}


