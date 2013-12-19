using System.Drawing;
using VisualImageDiff.ColorStructures;

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
                    IColor colorLeft = left.GetPixelColor(x, y);
                    IColor colorRight = right.GetPixelColor(x, y);
                    byte grayScale = GetGrayScaleDiff(colorLeft, colorRight);
                    Color colorDiff = Color.FromArgb(255, grayScale, grayScale, grayScale);
                    diff.SetPixelColor(x, y, colorDiff);
                    // TODO heatmap instead of grayscale
                }
            }
        }

        protected abstract byte GetGrayScaleDiff(IColor left, IColor right);
    }
}
