using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    public class MsdnHsbBDiffCurve : CurveDiff
    {
        public override String Name { get { return "Msdn HSB B Diff Curve"; } }

        protected override int GetYvalue(Color left, Color right, int yMax)
        {
            float bLeft = left.GetBrightness();          // belongs to [0.0,1.0]
            float bRight = right.GetBrightness();
            float diff = bRight - bLeft;        // diff belongs to [-1.0, 1.0]
            double diff2 = diff + 1.0;    // diff2 belongs to [0.0, 2.0]
            double y = yMax * diff2 / 2.0;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }
}
