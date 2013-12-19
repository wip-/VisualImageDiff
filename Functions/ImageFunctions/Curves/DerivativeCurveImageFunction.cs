using System;
using System.Drawing;
using System.Linq;
using VisualImageDiff.ColorStructures;
using VisualImageDiff.Functions;
using VisualImageDiff.Functions.ColorFunctions;

namespace VisualImageDiff.ImageFunctions
{
    /// <summary>
    /// Base class for functions that output a curve
    /// For each x value of input 1d images, output a y value
    /// </summary>
    public class DerivativeCurveImageFunction<T> : IImageFunction, ICurveFunction
        where T : BaseColorFunction, new()
    {
        private T t;

        public DerivativeCurveImageFunction()
        {
            t = new T();
        }

        public String Name { get { return "Derivative of " + t.Name; } }

        public double[] CurveValues { get; protected set; }


        public void FillImage(BitmapInfo source, BitmapInfo dest)
        {
            // vertical Y value where image is sampled
            // TODO make it a parameter
            int Ysample = source.Height/2;

            CurveValues = new double[dest.Width - 1];

            for (int x = 0; x < dest.Width-1; x++)
            {
                IColor colorSource0 = source.GetPixelColor(x, Ysample);
                IColor colorSource1 = source.GetPixelColor(x + 1, Ysample);

                double yVal0 = t.GetYvalue(colorSource0);    // belongs to [t.yInputMin, t.yInputMax]
                double yVal1 = t.GetYvalue(colorSource1);    // belongs to [t.yInputMin, t.yInputMax]
                CurveValues[x] = yVal1 - yVal0;              // belongs to [t.yInputMin-t.yInputMax, t.yInputMax-t.yInputMin]
            }

            // scale derivative values to fully fill the image
            double min = CurveValues.Min();
            double max = CurveValues.Max();

            int[] quantizedValues = new int[dest.Width - 1];
            for (int x = 0; x < dest.Width - 1; x++)
            {
                double scaledValue = Helper.Lerp(
                    CurveValues[x],
                    min, max, 0, dest.Height - 1);          // belongs to [0, dest.Height[
                quantizedValues[x] = Convert.ToInt32(scaledValue.Clamp(0, dest.Height - 1));
            }
                


            for (int x = 0; x < dest.Width; x++)
            {
                int yDrv = -1;

                if (x < dest.Width - 1)
                    yDrv = quantizedValues[x];

                // in the original image y is higher at the bottom
                // we invert y so that output curves are easier to understand
                for (int y = 0; y < dest.Height; ++y)
                {
                    if (y == yDrv)
                        dest.SetPixelColor(x, (dest.Height - 1) - y, Color.Black);
                    else
                        dest.SetPixelColor(x, (dest.Height - 1) - y, Color.White);
                }
            }
        }
    }
}
