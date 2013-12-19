using System;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.DiffFunctions
{
    public class MsdnHsbHDiffGrayscale : GrayscaleDiff
    {
        public override String Name { get { return "Msdn HSB H Diff Grayscale"; } }

        protected override byte GetGrayScaleDiff(IColor left, IColor right)
        {
            float hueLeft = left.ToMsdnColor().GetHue();     // In degrees, [0.0-360.0]
            float hueRight = right.ToMsdnColor().GetHue();

            float hueDiff = Math.Max(hueLeft, hueRight) - Math.Min(hueLeft, hueRight);
            if (hueDiff > 180.0)
                hueDiff = 360f - hueDiff;  // In degrees, [0.0-180.0]

            float diff255 = 255 - 255 * hueDiff / 180;
            byte grayScale = Convert.ToByte(diff255.Clamp0_255());

            return grayScale;
        }
    }
}
