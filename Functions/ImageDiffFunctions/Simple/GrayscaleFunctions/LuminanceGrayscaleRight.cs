using System;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.DiffFunctions
{
    class LuminanceGrayscaleRight : GrayscaleDiff
    {
        public override String Name { get { return "Luminance Grayscale right"; } }

        protected override byte GetGrayScaleDiff(IColor left, IColor right)
        {
            double luminance = Helper.GetRelativeLuminance(right);
            double luminance256 = 256 * luminance;
            byte grayScale = Convert.ToByte(luminance256.Clamp0_255());
            return grayScale;
        }
    }
}
