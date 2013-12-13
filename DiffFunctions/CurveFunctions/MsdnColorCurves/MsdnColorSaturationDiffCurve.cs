using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    public class MsdnHsbSDiffCurve : CurveFunction
    {
        public override String Name { get { return "Msdn HSB S Diff Curve"; } }

        protected override int GetYvalue(Color left, Color right, int yMax)
        {
            float satLeft = left.GetSaturation();          // belongs to [0.0,1.0]
            float satRight = right.GetSaturation();
            float diff = satRight - satLeft;        // diff belongs to [-1.0, 1.0]
            double diff2 = diff + 1.0;    // diff2 belongs to [0.0, 2.0]
            double y = yMax * diff2 / 2.0;
            return Convert.ToInt32(y.Clamp(0, yMax - 1));
        }
    }
}
