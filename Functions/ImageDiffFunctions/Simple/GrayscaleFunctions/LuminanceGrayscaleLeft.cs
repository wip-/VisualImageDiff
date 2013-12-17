using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    class LuminanceGrayscaleLeft : GrayscaleDiff
    {
        public override String Name { get { return "Luminance Grayscale left"; } }

        protected override byte GetGrayScaleDiff(Color left, Color right)
        {
            double luminance = Helper.GetRelativeLuminance(left);
            byte grayScale = Convert.ToByte(luminance.Clamp0_255());
            return grayScale;
        }
    }
}
