using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    public class RedRatioCurve : CurveDiff
    {
        public override String Name { get { return "Red Ratio Curve"; } }

        protected override int GetYvalue(Color left, Color right, int yMax)
        {
            int y = yMax;
            if (left.R > 0)
            {
                double ratio = (double)right.R / (double)left.R;     // ratio belongs to [1/255, 255]

                // but we clamp it to 4 times input color
                double ratioClamped = ratio.Clamp(0.0, 4.0);
                double ratioScaled = yMax * ratio / 4.0;
                return Convert.ToInt32(ratioScaled);
            }
            return y;

        }
    }
}
