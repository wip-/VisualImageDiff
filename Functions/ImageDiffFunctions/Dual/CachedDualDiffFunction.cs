using System;
using System.Drawing.Imaging;

namespace VisualImageDiff.DiffFunctions
{
    public abstract class CachedDualDiffFunction : IDualDiffFunction //, ICachedDiffFunction
    {
        public abstract String Name { get; }

        private BitmapInfoPair cachedBitmapInfoDiffPair;

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
        public virtual BitmapInfoPair CreateDiff(BitmapInfo left, BitmapInfo right)
        {
            return CreateDiff(left, right, EnableCache.True);
        }

        public BitmapInfoPair CreateDiff(BitmapInfo left, BitmapInfo right, EnableCache enableCache)
        {
            if (cachedBitmapInfoDiffPair != null)
                return cachedBitmapInfoDiffPair;

            BitmapInfo bitmapInfoDiffLeft =
                new BitmapInfo(left.Width, left.Height, left.PixelFormat);

            BitmapInfo bitmapInfoDiffRight =
                new BitmapInfo(right.Width, right.Height, right.PixelFormat);

            BitmapInfoPair bitmapInfoPair = new BitmapInfoPair(bitmapInfoDiffLeft, bitmapInfoDiffRight);

            FillDiffPair(bitmapInfoPair, left, right);

            if (enableCache == EnableCache.True)
                cachedBitmapInfoDiffPair = bitmapInfoPair;

            return bitmapInfoPair;
        }

        public void InvalidateCache()
        {
            cachedBitmapInfoDiffPair = null;
        }

        /// <summary>
        /// Do the actual work
        /// </summary>
        protected abstract void FillDiffPair(BitmapInfoPair bitmapInfoPair, BitmapInfo left, BitmapInfo right);
    }
}
