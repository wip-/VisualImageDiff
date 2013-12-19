using System;
using System.Drawing;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.DiffFunctions
{
    public class RedRatioCurve : CachedDiffFunction
    {
        public override String Name { get { return "Red Ratio Curve"; } }


        // TODO factorize with ColorDiffFunction<>
        // use Diff<>, Ratio<> functions
        // and Derivative<>
        protected override void FillDiff(BitmapInfo diff, BitmapInfo left, BitmapInfo right)
        {
            int yMax = diff.Height / 2;

            for (int x = 0; x < diff.Width; x++)
            {
                IColor colorLeft = left.GetPixelColor(x, diff.Height / 2);
                IColor colorRight = right.GetPixelColor(x, diff.Height / 2);

                int yVal = yMax;
                if (colorRight.R > 0)
                {
                    double ratio = (double)colorLeft.R / (double)colorRight.R;     // ratio belongs to [1/255, 255]

                    // but we clamp it to 4 times input color
                    double ratioClamped = ratio.Clamp(0.0, 4.0);
                    double ratioScaled = yMax * ratio / 4.0;
                    yVal = Convert.ToInt32(ratioScaled);
                }

                // in the original image y is higher at the bottom
                // we invert y so that output curves are easier to understand
                for (int y = 0; y < diff.Height; ++y)
                {
                    if (y == yVal)
                        diff.SetPixelColor(x, (diff.Height - 1) - y, Color.Black);
                    else
                        diff.SetPixelColor(x, (diff.Height - 1) - y, Color.White);
                }
            }
        }

    }
}
