using System;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.DiffFunctions
{
    class MsdnHsbSDiffGrayscale : GrayscaleDiff
    {
        public override String Name { get { return "Msdn HSB S Diff Grayscale"; } }

        protected override byte GetGrayScaleDiff(IColor left, IColor right)
        {
            float satLeft = left.ToMsdnColor().GetSaturation();     // In degrees, [0.0-360.0]
            float satRight = right.ToMsdnColor().GetSaturation();
            float hueDiff = Math.Max(satLeft, satRight) - Math.Min(satLeft, satRight);
            float diff255 = 255 - 255 * hueDiff;
            byte grayScale = Convert.ToByte(diff255.Clamp0_255());

            return grayScale;
        }
    }
}
