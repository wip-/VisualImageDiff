using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualImageDiff.DiffFunctions
{
    /// <summary>
    /// Base class for functions that output a grayscale value
    /// </summary>
    public abstract class GrayscaleDiff : CachedDiffFunction
    {
        protected override void FillDiff(BitmapInfo diff, BitmapInfo left, BitmapInfo right)
        {
            for (int y = 0; y < diff.Height; y++)
            {
                for (int x = 0; x < diff.Width; x++)
                {
                    Color colorLeft = left.GetPixelColor(x, y);
                    Color colorRight = right.GetPixelColor(x, y);
                    byte grayScale = GetGrayScaleDiff(colorLeft, colorRight);
                    Color colorDiff = Color.FromArgb(255, grayScale, grayScale, grayScale);
                    diff.SetPixelColor(x, y, colorDiff);
                    // TODO heatmap instead of grayscale
                }
            }
        }

        protected abstract byte GetGrayScaleDiff(Color left, Color right);
    }
}
