using System;
using VisualImageDiff.ImageFunctions;

namespace VisualImageDiff.DiffFunctions
{
    /// <summary>
    /// Do the same process on left and right images, outputs the 2 results
    /// </summary>
    public class DualProcess<T> : CachedDualDiffFunction
        where T : IImageFunction, new()
    {
        private T t;

        public DualProcess()
        {
            t = new T();
        }

        public override String Name { get { return t.Name; } }

        /// <summary>
        /// Do the actual work
        /// </summary>
        protected override void FillDiffPair(BitmapInfoPair bitmapInfoPair, BitmapInfo left, BitmapInfo right)
        {
            t.FillImage(left, bitmapInfoPair.left);
            t.FillImage(right, bitmapInfoPair.right);
        }
    }
}
