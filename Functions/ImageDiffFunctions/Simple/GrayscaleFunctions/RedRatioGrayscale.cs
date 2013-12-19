using System;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.DiffFunctions
{
    public class RedRatioGrayscale : GrayscaleDiff
    {
        public override String Name { get { return "Red Ratio Grayscale"; } }

        protected override byte GetGrayScaleDiff(IColor left, IColor right)
        {
            double r = (double)right.R / (double)left.R;    // belongs to [0, infinite]

            // remapping: a value between [0.5, 1.5] would be remapped to [0, 255]
            r = 255 * (r - 0.5);

            return Convert.ToByte(r.Clamp0_255());
        }
    }
}
