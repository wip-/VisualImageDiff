using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using VisualImageDiff.ColorStructures;
using VisualImageDiff.Helpers;

namespace VisualImageDiff
{
    public class BitmapInfoPair
    {
        public BitmapInfo left;
        public BitmapInfo right;
        public BitmapInfoPair(BitmapInfo left, BitmapInfo right)
        {
            this.left = left;
            this.right = right;
        }
    }


    public class BitmapInfo
    {
        public int Width;
        public int Height;
        public int Stride;
        public int DataByteSize;
        public byte[] Data;
        public PixelFormat PixelFormat;
        public int BitsPerPixel;
        public int BytesPerPixel;
        private ColorPalette palette;

        public enum CopyData
        {
            True,
            False
        }

        public Boolean HasAlphaChannel()
        {
            return Helper.HasAlphaChannel(PixelFormat);
        }

        public BitmapInfo(Bitmap bitmap, CopyData copyData = CopyData.True)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            PixelFormat = bitmap.PixelFormat;
            BitsPerPixel = Image.GetPixelFormatSize(PixelFormat);
            BytesPerPixel = Helper.GetBytesPerPixel(bitmap.PixelFormat);
            Rectangle rect = Rectangle.FromLTRB(0, 0, Width, Height);
            BitmapData bitmapData = bitmap.LockBits(rect,
                ImageLockMode.ReadOnly, PixelFormat);
            Stride = Math.Abs(bitmapData.Stride);
            DataByteSize = Stride * Height;
            Data = new byte[DataByteSize];
            if (copyData == CopyData.True)
                Marshal.Copy(bitmapData.Scan0, Data, 0, DataByteSize);
            bitmap.UnlockBits(bitmapData);

            if (PixelFormat == PixelFormat.Format8bppIndexed)
                palette = bitmap.Palette;
        }

        public BitmapInfo(int width, int height, PixelFormat pixelFormat)
        {
            Width = width;
            Height = height;
            PixelFormat = pixelFormat;
            BitsPerPixel = Image.GetPixelFormatSize(PixelFormat);
            BytesPerPixel = Helper.GetBytesPerPixel(PixelFormat);

            int bitsPerLine = BytesPerPixel * Width;
            double d = (bitsPerLine+3) / 4;
            double d_rounded = Math.Ceiling(d);
            Stride = 4 * Convert.ToInt32(d_rounded);

            DataByteSize = Stride * Height;
            Data = new byte[DataByteSize];
        }


        public Bitmap ToBitmap()
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            Rectangle rect = Rectangle.FromLTRB(0, 0, Width, Height);
            BitmapData bitmapData = bitmap.LockBits(rect,
                ImageLockMode.WriteOnly, PixelFormat);
            Marshal.Copy(Data, 0, bitmapData.Scan0, DataByteSize);
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }


        // Get index of pixel in Data array from its coordinates
        private Int32 GetPixelInternalIndex(int x, int y)
        {
            return (Stride * y) + (BytesPerPixel * x);
        }

        public IColor GetPixelColor(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return Color32.FromArgb(0, 0, 0, 0);

            int pixelIndex = GetPixelInternalIndex(x, y);
            
            if (PixelFormat == PixelFormat.Format8bppIndexed)
            {
                byte colorIndex = Data[pixelIndex];
                return (Color32)palette.Entries[colorIndex];
            }
            else if (PixelFormat == PixelFormat.Format64bppArgb)
            {
                UInt16 a = BitConverter.ToUInt16(Data, pixelIndex + 6);
                UInt16 r = BitConverter.ToUInt16(Data, pixelIndex + 4);
                UInt16 g = BitConverter.ToUInt16(Data, pixelIndex + 2);
                UInt16 b = BitConverter.ToUInt16(Data, pixelIndex + 0);

                return Color64.FromArgb(a, r, g, b);
            }
            else
            {
                byte A = HasAlphaChannel() ? Data[pixelIndex + 3] : (byte)255;
                byte R = Data[pixelIndex + 2];
                byte G = Data[pixelIndex + 1];
                byte B = Data[pixelIndex + 0];

                return Color32.FromArgb(A, R, G, B);
            }
        }

        public void SetPixelColor(int x, int y, Color color)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return;

            int index = GetPixelInternalIndex(x, y);
            Data[index + 0] = color.B;  // B
            Data[index + 1] = color.G;  // G
            Data[index + 2] = color.R;  // R
            if (HasAlphaChannel())
                Data[index + 3] = color.A;  // A
        }



    }
}
