using System;
using System.Drawing;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.DiffFunctions
{
    // function to test various stuff on two images
    public class Prototype : IParamFunction
    {
        public String Name { get { return "prototype"; } }

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

                    float ratio;
                    #if true
                    // change in hue as a heat map
                    {
                        var hueLeft = colorLeft.GetHue();
                        var hueRight = colorRight.GetHue();
                        ratio = hueRight - hueLeft;
                        if( ratio > 180)
                            ratio -= 360;
                        if( ratio < -180)
                            ratio += 360;
                        
                        ratio /= 180;
                    }
                    #else
                    // change in saturation as a heat map
                    {
                        var satLeft = colorLeft.GetSaturation();
                        var satRight = colorRight.GetSaturation();
                        ratio = satRight - satLeft;
                    }
                    #endif

                    // if value becomes smaller: blue
                    // if value becomes bigger : red
                    // if value is same        : white
                    Color heatColor;
                    if (ratio > 0)
                    {
                        heatColor = Helper.Lerp(Color.White,Color.Red,ratio);
                    }
                    else
                    {
                        heatColor = Helper.Lerp(Color.White,Color.Blue,-ratio);
                    }

                    diff.SetPixelColor(x, y, heatColor);
                }
            }

            return diff;
        }
    }
}
