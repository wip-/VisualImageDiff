using System;
using System.Drawing;

namespace VisualImageDiff.DiffFunctions
{
    class LuminanceGrayscaleDiff : GrayscaleDiff
    {
        public override String Name { get { return "Luminance Grayscale diff"; } }

        protected override byte GetGrayScaleDiff(Color left, Color right)
        {
            double lumLeft = Helper.GetRelativeLuminance(left);
            double lumRight = Helper.GetRelativeLuminance(right);
            double lumDiff512 = lumRight - lumLeft;
            double lumDiff256 = 0.5 * (lumDiff512 + 256);
            byte grayScale = Convert.ToByte(lumDiff256.Clamp0_255());
            return grayScale;
        }
    }
}
