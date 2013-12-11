using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    public class HsvColorSDiffCurve : CurveDiff
    {
        public override String Name { get { return "Hsv S Diff Curve"; } }

        protected override int GetYvalue(Color left, Color right, int yMax)
        {
            double hLeft, hRight, sLeft, sRight, vLeft, vRight;
            Helper.ColorToHSV(left, out hLeft, out sLeft, out vLeft);
            Helper.ColorToHSV(right, out hRight, out sRight, out vRight);

            double diff = sRight - sLeft;        // diff belongs to [-1.0, 1.0]
            double diff2 = diff + 1.0;    // diff2 belongs to [0.0, 2.0]
            double y = yMax * diff2 / 2.0;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }
}
