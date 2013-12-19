using System;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.DiffFunctions
{
    class LuminanceGrayscaleDiff : GrayscaleDiff
    {
        public override String Name { get { return "Luminance Grayscale diff"; } }

        protected override byte GetGrayScaleDiff(IColor left, IColor right)
        {
            double lumLeft = Helper.GetRelativeLuminance(left);
            double lumRight = Helper.GetRelativeLuminance(right);   // belongs to [0, 1]
            double lumDiff = lumRight - lumLeft;                    // belongs to [-1, 1]
            double lumDiff256 = 128 * (lumDiff + 1);
            byte grayScale = Convert.ToByte(lumDiff256.Clamp0_255());
            return grayScale;
        }
    }
}
