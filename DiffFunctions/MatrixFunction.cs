//using System;
//using System.Drawing.Imaging;

//namespace VisualImageDiff.DiffFunctions
//{
//    /// <summary>
//    /// For each pixel, stores the 3x3 matrix that transforms left RGB color to right RGB color in a linear way
//    /// </summary>
//    class MatrixFunction : CachedDiffFunction
//    {
//        public override String Name { get { return "Matrix solve"; } }



//        protected override void FillDiff(BitmapInfo diff, BitmapInfo left, BitmapInfo right)
//        {
//            for (int y = 0; y < diff.Height; y++)
//            {
//                for (int x = 0; x < diff.Width; x++)
//                {
//                    Color colorLeft = left.GetPixelColor(x, y);
//                    Color colorRight = right.GetPixelColor(x, y);



//                }
//            }
//        }
//    }
//}
