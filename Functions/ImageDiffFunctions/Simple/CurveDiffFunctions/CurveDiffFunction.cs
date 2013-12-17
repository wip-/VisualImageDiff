using System;
using System.Drawing;
using VisualImageDiff.Functions.ColorFunctions;

namespace VisualImageDiff.DiffFunctions
{
    /// <summary>
    /// Base class for functions that output a curve
    /// For each x value of input 1d images, output a y value
    /// </summary>
    public class ColorDiffFunction<T> : CachedDiffFunction
        where T : IColorFunction, new()
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
                Color colorLeft = left.GetPixelColor(x, diff.Height/2);
                Color colorRight = right.GetPixelColor(x, diff.Height/2);

                int yLeft = t.GetYvalue(colorLeft, diff.Height);        // belongs to [0, Height]
                int yRight = t.GetYvalue(colorRight, diff.Height);     // belongs to [0, Height]
                int yVal = yRight - yLeft;                              // belongs to [-Height, Height]
                yVal = (yVal + diff.Height) / 2;

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
