using System;
using System.Drawing.Imaging;

namespace VisualImageDiff.DiffFunctions
{
    public abstract class CachedDiffFunction : ISimpleDiffFunction //, ICachedDiffFunction
    {
        public abstract String Name { get; }

        private BitmapInfo cachedBitmapInfoDiff;

        public enum EnableCache
        {
            True,
            False
        }

        /// <summary>
        /// Return cached bitmapinfo if already available
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual BitmapInfo CreateDiff(BitmapInfo left, BitmapInfo right)
        {
            return CreateDiff(left, right, EnableCache.True);
        }

        public BitmapInfo CreateDiff(BitmapInfo left, BitmapInfo right, EnableCache enableCache)
        {
            if (cachedBitmapInfoDiff != null)
                return cachedBitmapInfoDiff;

            BitmapInfo bitmapInfoDiff = new BitmapInfo(
                Math.Max(left.Width, right.Width),
                Math.Max(left.Height, right.Height),
                PixelFormat.Format24bppRgb);
            FillDiff(bitmapInfoDiff, left, right);
            
            if (enableCache == EnableCache.True)
                cachedBitmapInfoDiff = bitmapInfoDiff;

            return bitmapInfoDiff;
        }

        public void InvalidateCache()
        {
            cachedBitmapInfoDiff = null;
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
