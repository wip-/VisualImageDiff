using System;

namespace VisualImageDiff.ImageFunctions
{
    /// <summary>
    /// Analyze only a single image
    /// Return one processed image
    /// </summary>
    public interface IImageFunction
    {
        String Name { get; }
        void FillImage(BitmapInfo bitmapInfoSource, BitmapInfo bitmapInfoDest);
    }
}
