using System;
using System.Drawing;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.DiffFunctions
{
    public class Blend : IParamFunction
    {
        public String Name { get { return "blend"; } }

        public double Parameter { get; set; }

        public BitmapInfo CreateDiff(BitmapInfo left, BitmapInfo right)
        {
            BitmapInfo diff = new BitmapInfo(
                Math.Max(left.Width, right.Width),
                Math.Max(left.Height, right.Height),
                left.PixelFormat);

            for (int y = 0; y < diff.Height; y++)
            {
                for (int x = 0; x < diff.Width; x++)
                {
                    Color colorLeft = left.GetPixelColor(x, y).ToMsdnColor();
                    Color colorRight = right.GetPixelColor(x, y).ToMsdnColor();

                    //Color colorBlend = Helper.Lerp(colorLeft, colorRight, 1 - colorLeft.A/255);

                    Color colorBlend = Helper.Lerp(colorLeft, colorRight, Parameter);

                    diff.SetPixelColor(x, y, colorBlend);
                }
            }

            return diff;
        }
    }
}
