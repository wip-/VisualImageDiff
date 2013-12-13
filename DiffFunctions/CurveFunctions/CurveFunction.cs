using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualImageDiff.DiffFunctions
{
    /// <summary>
    /// Base class for functions that output a curve
    /// For each x value of input 1d images, output a y value
    /// </summary>
    public abstract class CurveFunction : CachedDiffFunction
    {
        protected override void FillDiff(BitmapInfo diff, BitmapInfo left, BitmapInfo right)
        {
            for (int x = 0; x < diff.Width; x++)
            {
                Color colorLeft = left.GetPixelColor(x, diff.Height/2);
                Color colorRight = right.GetPixelColor(x, diff.Height/2);
                int yVal = GetYvalue(colorLeft, colorRight, diff.Height);

                for (int y = 0; y < diff.Height; ++y )
                {
                    if(y==yVal)
                        diff.SetPixelColor(x, y, Color.Black);
                    else
                        diff.SetPixelColor(x, y, Color.White);
                }
            }
        }

        protected abstract int GetYvalue(Color left, Color right, int yMax);
    }
}
