using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    public class HueCurveLeft : CurveFunction
    {
        public override String Name { get { return "Hue Curve Left"; } }

        protected override int GetYvalue(Color left, Color right, int yMax)
        {
            float hue = left.GetHue();  // [0,360]
            double y = yMax * hue/360;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }
}
