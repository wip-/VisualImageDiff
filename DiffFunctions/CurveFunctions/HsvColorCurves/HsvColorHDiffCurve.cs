using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    public class HsvColorHDiffCurve : CurveDiff
    {
        public override String Name { get { return "Hsv H Diff Curve"; } }

        protected override int GetYvalue(Color left, Color right, int yMax)
        {
            double hLeft, hRight, sLeft, sRight, vLeft, vRight;
            Helper.ColorToHSV(left, out hLeft, out sLeft, out vLeft);
            Helper.ColorToHSV(right, out hRight, out sRight, out vRight);

            double diff = hRight - hLeft;        // diff belongs to [-360, 360]
            double diff720 = diff + 360.0;    // diff512 belongs to [0, 720]
            double y = yMax * diff720 / 720.0;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }
}
