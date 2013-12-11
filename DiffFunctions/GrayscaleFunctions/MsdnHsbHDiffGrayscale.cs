using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualImageDiff.DiffFunctions
{
    public class MsdnHsbHDiffGrayscale : GrayscaleDiff
    {
        public override String Name { get { return "Msdn HSB H Diff Grayscale"; } }

        protected override byte GetGrayScaleDiff(Color left, Color right)
        {
            float hueLeft = left.GetHue();     // In degrees, [0.0-360.0]
            float hueRight = right.GetHue();

            float hueDiff = Math.Max(hueLeft, hueRight) - Math.Min(hueLeft, hueRight);
            if (hueDiff > 180.0)
                hueDiff = 360f - hueDiff;  // In degrees, [0.0-180.0]

            float diff255 = 255 - 255 * hueDiff / 180;
            byte grayScale = Convert.ToByte(diff255.Clamp0_255());

            return grayScale;
        }
    }
}
