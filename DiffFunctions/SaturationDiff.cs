using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualImageDiff.DiffFunctions
{
    class SaturationDiff : GrayScaleDiff
    {
        public override String Name { get { return "Saturation Diff"; } }

        protected override byte GetGrayScaleDiff(Color left, Color right)
        {
            float satLeft = left.GetSaturation();     // In degrees, [0.0-360.0]
            float satRight = right.GetSaturation();
            float hueDiff = Math.Max(satLeft, satRight) - Math.Min(satLeft, satRight);
            float diff255 = 255 - 255 * hueDiff;
            byte grayScale = Convert.ToByte(diff255.Clamp0_255());

            return grayScale;
        }
    }
}
