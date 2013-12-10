using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualImageDiff.DiffFunctions
{
    public abstract class CachedDiffFunction : IDiffFunction
    {
        public abstract String Name { get; }

        private BitmapInfo bitmapInfoDiff;

        /// <summary>
        /// Return cached bitmapinfo if already available
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public BitmapInfo CreateDiff(BitmapInfo left, BitmapInfo right)
        {
            if (bitmapInfoDiff == null)
            {
                bitmapInfoDiff = new BitmapInfo(
                    Math.Max(left.Width, right.Width),
                    Math.Max(left.Height, right.Height),
                    PixelFormat.Format24bppRgb);
                FillDiff(bitmapInfoDiff, left, right);
            }
            return bitmapInfoDiff;
        }

        /// <summary>
        /// Do the actual work
        /// </summary>
        /// <param name="diff"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        protected abstract void FillDiff(BitmapInfo diff, BitmapInfo left, BitmapInfo right);
    }
}
