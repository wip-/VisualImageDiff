using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    public class BlueDiffCurve : CurveDiff
    {
        public override String Name { get { return "Blue Diff Curve"; } }

        protected override int GetYvalue(Color left, Color right, int yMax)
        {
            int diff = (int)right.B - (int)left.B;     // diff belongs to [-255, 255]
            double diff512 = (double)diff + 256.0;    // diff512 belongs to [1, 511]
            double y = yMax * diff512 / 512.0;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }
}
