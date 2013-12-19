using System;
using System.Drawing;
using VisualImageDiff.ColorStructures;
using VisualImageDiff.Functions;
using VisualImageDiff.Functions.ColorFunctions;

namespace VisualImageDiff.ImageFunctions
{
    /// <summary>
    /// Base class for functions that output a curve
    /// For each x value of input 1d images, output a y value
    /// </summary>
    public class CurveImageFunction<T> : IImageFunction, ICurveFunction
        where T: BaseColorFunction, new()
    {
        private T t;

        public CurveImageFunction()
        {
            t = new T();
        }

        public String Name { get { return t.Name; } }

        public double[] CurveValues { get; protected set; }

        public void FillImage(BitmapInfo source, BitmapInfo dest)
        {
            // vertical Y value where image is sampled
            // TODO make it a parameter
            int Ysample = source.Height/2;

            CurveValues = new double[dest.Width];
            int[] quantizedValues = new int[dest.Width];
            for (int x = 0; x < dest.Width; x++)
            {
                IColor colorSource = source.GetPixelColor(x, Ysample);
                double yVal = t.GetYvalue(colorSource);
                CurveValues[x] = yVal;
                double scaledValue = Helper.Lerp(yVal, t.yInputMin, t.yInputMax, 0, dest.Height-1);
                quantizedValues[x] = Convert.ToInt32(scaledValue.Clamp(0, dest.Height - 1));
            }

            for (int x = 0; x < dest.Width; x++)
            {
                IColor colorSource = source.GetPixelColor(x, Ysample);
                int yVal = quantizedValues[x];

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
