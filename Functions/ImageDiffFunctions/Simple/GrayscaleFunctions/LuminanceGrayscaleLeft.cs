using System;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.DiffFunctions
{
    class LuminanceGrayscaleLeft : GrayscaleDiff
    {
        public override String Name { get { return "Luminance Grayscale left"; } }

        protected override byte GetGrayScaleDiff(IColor left, IColor right)
        {
            double luminance = Helper.GetRelativeLuminance(left);
            double luminance256 = 256 * luminance;
            byte grayScale = Convert.ToByte(luminance256.Clamp0_255());
            return grayScale;
        }
    }
}
