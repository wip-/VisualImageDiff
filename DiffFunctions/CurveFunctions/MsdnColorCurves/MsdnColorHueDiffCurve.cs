using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    public class MsdnHsbHDiffCurve : CurveFunction
    {
        public override String Name { get { return "Msdn HSB H Diff Curve"; } }

        protected override int GetYvalue(Color left, Color right, int yMax)
        {
            float hueLeft = left.GetHue();          // In degrees, [0.0-360.0]
            float hueRight = right.GetHue();
            float diff = hueRight - hueLeft;        // diff belongs to [-360, 360]
            double diff720 = diff + 360.0;    // diff512 belongs to [0, 720]
            double y = yMax * diff720 / 720.0;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }
}
