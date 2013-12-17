using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    class LuminanceGrayscaleRight : GrayscaleDiff
    {
        public override String Name { get { return "Luminance Grayscale right"; } }

        protected override byte GetGrayScaleDiff(Color left, Color right)
        {
            double luminance = Helper.GetRelativeLuminance(right);
            byte grayScale = Convert.ToByte(luminance.Clamp0_255());
            return grayScale;
        }
    }
}
