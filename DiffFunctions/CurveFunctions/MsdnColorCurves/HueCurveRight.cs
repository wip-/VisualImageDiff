using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    public class HueCurveRight : CurveFunction
    {
        public override String Name { get { return "Hue Curve Right"; } }

        protected override int GetYvalue(Color left, Color right, int yMax)
        {
            float hue = right.GetHue();  // [0,360]
            double y = yMax * hue / 360;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }
}
