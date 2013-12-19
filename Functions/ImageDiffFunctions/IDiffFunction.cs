using System;

namespace VisualImageDiff.DiffFunctions
{
    public interface IDiffFunction
    {
        String Name { get; }
    }

    //public interface ICachedDiffFunction
    //{
    //    public enum EnableCache
    //    {
    //        True,
    //        False
    //    }
    //    void InvalidateCache();
    //}

    /// <summary>
    /// Analyze two images
    /// Return one result image
    /// </summary>
    public interface ISimpleDiffFunction : IDiffFunction
    {
        BitmapInfo CreateDiff(BitmapInfo left, BitmapInfo right);
    }

    /// <summary>
    /// Analyze two images
    /// Return two images
    /// </summary>
    public interface IDualDiffFunction : IDiffFunction
    {
        BitmapInfoPair CreateDiff(BitmapInfo left, BitmapInfo right);
    }
}
