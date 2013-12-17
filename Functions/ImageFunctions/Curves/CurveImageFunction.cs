using System;
using System.Drawing;
using VisualImageDiff.Functions.ColorFunctions;

namespace VisualImageDiff.ImageFunctions
{
    /// <summary>
    /// Base class for functions that output a curve
    /// For each x value of input 1d images, output a y value
    /// </summary>
    public class CurveImageFunction<T> : IImageFunction
        where T: IColorFunction, new()
    {
        private T t;

        public CurveImageFunction()
        {
            t = new T();
        }

        public String Name { get { return t.Name; } }

        public void FillImage(BitmapInfo source, BitmapInfo dest)
        {
            // vertical Y value where image is sampled
            // TODO make it a parameter
            int Ysample = source.Height/2;

            for (int x = 0; x < dest.Width; x++)
            {
                Color colorSource = source.GetPixelColor(x, Ysample);
                int yVal = t.GetYvalue(colorSource, dest.Height);

                // in the original image y is higher at the bottom
                // we invert y so that output curves are easier to understand
                for (int y = 0; y < dest.Height; ++y)
                {
                    if(y==yVal)
                        dest.SetPixelColor(x, (dest.Height - 1) - y, Color.Black);
                    else
                        dest.SetPixelColor(x, (dest.Height - 1) - y, Color.White);
                }
            }
        }
    }
}
