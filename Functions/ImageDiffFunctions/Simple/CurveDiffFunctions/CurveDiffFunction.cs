using System;
using System.Drawing;
using VisualImageDiff.ColorStructures;
using VisualImageDiff.Functions.ColorFunctions;

namespace VisualImageDiff.DiffFunctions
{
    /// <summary>
    /// Base class for functions that output a curve
    /// For each x value of input 1d images, output a y value
    /// </summary>
    public class ColorDiffFunction<T> : CachedDiffFunction
        where T : BaseColorFunction, new()
    {
        private T t;

        public override String Name { get { return t.Name + " diff"; } }

        public ColorDiffFunction()
        {
            t = new T();
        }

        protected override void FillDiff(BitmapInfo diff, BitmapInfo left, BitmapInfo right)
        {
            for (int x = 0; x < diff.Width; x++)
            {
                IColor colorLeft = left.GetPixelColor(x, diff.Height/2);
                IColor colorRight = right.GetPixelColor(x, diff.Height/2);

                double yLeft = t.GetYvalue(colorLeft);      // belongs to [t.yInputMin, t.yInputMax]
                double yRight = t.GetYvalue(colorRight);    // belongs to [t.yInputMin, t.yInputMax]
                double yDiff = yRight - yLeft;              // belongs to [t.yInputMin-t.yInputMax, t.yInputMax-t.yInputMin]

                double yValRemapped = Helper.Lerp(yDiff, 
                    (t.yInputMin-t.yInputMax), (t.yInputMax-t.yInputMin),
                    0, (diff.Height-1));

                int yVal = Convert.ToInt32(yValRemapped.Clamp(0, diff.Height - 1));

                // in the original image y is higher at the bottom
                // we invert y so that output curves are easier to understand
                for (int y = 0; y < diff.Height; ++y )
                {
                    if(y==yVal)
                        diff.SetPixelColor(x, (diff.Height-1)-y, Color.Black);
                    else
                        diff.SetPixelColor(x, (diff.Height-1)-y, Color.White);
                }
            }
        }
    }
}
