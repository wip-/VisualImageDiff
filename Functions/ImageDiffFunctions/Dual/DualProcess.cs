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
        public T LeftImageFunction { get; protected set; }
        public T RightImageFunction { get; protected set; }

        public DualProcess()
        {
            LeftImageFunction = new T();
            RightImageFunction = new T();
        }

        public override String Name { get { return LeftImageFunction.Name; } }

        /// <summary>
        /// Do the actual work
        /// </summary>
        protected override void FillDiffPair(BitmapInfoPair bitmapInfoPair, BitmapInfo left, BitmapInfo right)
        {
            LeftImageFunction.FillImage(left, bitmapInfoPair.left);
            RightImageFunction.FillImage(right, bitmapInfoPair.right);
        }
    }
}
