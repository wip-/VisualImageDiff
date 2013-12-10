using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VisualImageDiff
{
    public class BitmapInfo
    {
        public int Width;
        public int Height;
        public int Stride;
        public int Components;
        public int DataByteSize;
        public byte[] Data;
        public PixelFormat PixelFormat;
        private ColorPalette palette;

        public enum CopyData
        {
            True,
            False
        }

        public BitmapInfo(Bitmap bitmap, CopyData copyData = CopyData.True)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            Components = Helper.GetComponentsNumber(bitmap.PixelFormat);
            PixelFormat = bitmap.PixelFormat;

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
            Components = Helper.GetComponentsNumber(PixelFormat);
            double d = Width / 4;
            Stride = Components * 4 * Convert.ToInt32(Math.Round(d));
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
            return (Stride * y) + (Components * x);
        }

        public Color GetPixelColor(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return Color.FromArgb(0, 0, 0, 0);

            int pixelIndex = GetPixelInternalIndex(x, y);
            
            if (PixelFormat == PixelFormat.Format8bppIndexed)
            {
                byte colorIndex = Data[pixelIndex];
                return palette.Entries[colorIndex];
            }
            else
            {
                byte A = (Components == 4) ? Data[pixelIndex + 3] : (byte)255;
                byte R = Data[pixelIndex + 2];
                byte G = Data[pixelIndex + 1];
                byte B = Data[pixelIndex + 0];

                return Color.FromArgb(A, R, G, B);
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
            if (Components == 4)
                Data[index + 3] = color.A;  // A
        }



    }
}
