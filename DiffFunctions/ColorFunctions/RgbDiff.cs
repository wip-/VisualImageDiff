using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualImageDiff.DiffFunctions
{
    public class RgbDiff : CachedDiffFunction
    {
        public override String Name { get { return "RGB Diff"; } }

        protected override void FillDiff(BitmapInfo diff, BitmapInfo left, BitmapInfo right)
        {
            for (int y = 0; y < diff.Height; y++)
            {
                for (int x = 0; x < diff.Width; x++)
                {
                    Color colorLeft = left.GetPixelColor(x, y);
                    Color colorRight = right.GetPixelColor(x, y);
                    Color colorDiff = Helper.GetColorDiff(colorLeft, colorRight);
                    diff.SetPixelColor(x, y, colorDiff);
                }
            }

            // remap colors -> TODO
            // TODO also: a heat map version / combobox to select different visualization modes
        }
    }
}
