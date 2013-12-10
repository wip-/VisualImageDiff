﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Controls;

//using ScrollChangedEventArgs = System.Windows.Controls.ScrollChangedEventArgs;
//using ScrollViewer = System.Windows.Controls.ScrollViewer;



namespace VisualImageDiff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap bitmapLeft;
        Bitmap bitmapRight;

        BitmapInfo bitmapInfoLeft;
        BitmapInfo bitmapInfoRight;
        BitmapInfo bitmapInfoDiff;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyCatch(System.Exception ex)
        {
            var st = new StackTrace(ex, true);      // stack trace for the exception with source file information
            var frame = st.GetFrame(0);             // top stack frame
            String sourceMsg = String.Format("{0}({1})", frame.GetFileName(), frame.GetFileLineNumber());
            Console.WriteLine(sourceMsg);
            MessageBox.Show(ex.Message + Environment.NewLine + sourceMsg);
            Debugger.Break();
        }

        private void ImageLeft_Drop(object sender, DragEventArgs e)
        {
            String msg = LoadImage(ImageLeft, ref bitmapLeft, e);

            if (msg != null)
                LabelInfo.Content = msg;
            else
            {
                bitmapInfoLeft = new BitmapInfo(bitmapLeft);
                CreateDiff();
            }

        }

        private void ImageRight_Drop(object sender, DragEventArgs e)
        {
            String msg = LoadImage(ImageRight, ref bitmapRight, e);

            if (msg != null)
                LabelInfo.Content = msg;
            else
            {
                bitmapInfoRight = new BitmapInfo(bitmapRight);
                CreateDiff();
            }
        }


        private void CreateDiff()
        {
            if (bitmapLeft != null && bitmapRight != null)
            {
                bitmapInfoDiff = BitmapInfo.CreateDiff(bitmapInfoLeft, bitmapInfoRight);
                ImageDiff.Source =
                    Imaging.CreateBitmapSourceFromHBitmap(
                    bitmapInfoDiff.ToBitmap().GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }


        private String LoadImage(
            System.Windows.Controls.Image destinationImage,
            ref Bitmap destinationBitmap, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return "Not a file!";

                String[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 1)
                    return "Too many files!";

                String imageSourceFileName = files[0];

                if (!File.Exists(imageSourceFileName))
                    return "Not a file!";

                FileStream fs = null;
                try
                {
                    fs = File.Open(imageSourceFileName, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (IOException)
                {
                    if (fs != null)
                        fs.Close();
                    return "File already in use!";
                }

                try
                {
                    destinationBitmap = new Bitmap(fs);
                }
                catch (System.Exception /*ex*/)
                {
                    destinationBitmap.Dispose();
                    return "Not an image!";
                }

                destinationImage.Source =
                    Imaging.CreateBitmapSourceFromHBitmap(
                        destinationBitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());



                return null;
            }
            catch (System.Exception ex)
            {
                MyCatch(ex);
                return "Exception";
            }
        }




        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            double vert = ((ScrollViewer)sender).VerticalOffset;
            double hori = ((ScrollViewer)sender).HorizontalOffset;

            ScrollViewer[] scrollViewers = new ScrollViewer[]
                { ScrollViewerImageLeft, ScrollViewerImageRight, ScrollViewerImageDiff};

            foreach (ScrollViewer scrollViewer in scrollViewers )
            {
                scrollViewer.ScrollToVerticalOffset(vert);
                scrollViewer.ScrollToHorizontalOffset(hori);
                scrollViewer.UpdateLayout();
            }
        }

        private void SliderZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                ScaleTransform myScaleTransform = new ScaleTransform();
                myScaleTransform.ScaleY = SliderZoom.Value;
                myScaleTransform.ScaleX = SliderZoom.Value;
                if (LabelZoom != null)
                    LabelZoom.Content = SliderZoom.Value;
                TransformGroup myTransformGroup = new TransformGroup();
                myTransformGroup.Children.Add(myScaleTransform);

                System.Windows.Controls.Image[] images =
                    new System.Windows.Controls.Image[] { ImageLeft, ImageRight, ImageDiff };

                foreach (System.Windows.Controls.Image image in images)
                {
                    if (image == null || image.Source == null)
                        continue;
                    //image.RenderTransform = myTransformGroup;
                    image.LayoutTransform = myTransformGroup;
                }
            }
            catch (System.Exception ex)
            {
                MyCatch(ex);
            }
        }


        private void ButtonResetZoom_Click(object sender, RoutedEventArgs e)
        {
            SliderZoom.Value = 1;
        }


        private void ImageLeft_MouseMove(object sender, MouseEventArgs e)
        {
            Image_MouseMove(ImageLeft, e);
        }

        private void ImageRight_MouseMove(object sender, MouseEventArgs e)
        {
            Image_MouseMove(ImageRight, e);
        }

        private void ImageDiff_MouseMove(object sender, MouseEventArgs e)
        {
            Image_MouseMove(ImageDiff, e);
        }


        private void Image_MouseMove(System.Windows.Controls.Image clickedImage, MouseEventArgs e)
        {
            int x = (int)(e.GetPosition(clickedImage).X);
            int y = (int)(e.GetPosition(clickedImage).Y);

            BitmapInfo[] bitmapInfos =
                new BitmapInfo[] { bitmapInfoLeft, bitmapInfoRight, bitmapInfoDiff };

            System.Windows.Controls.Label[] labels =
                new System.Windows.Controls.Label[] { LabelColorLeft, LabelColorRight, LabelColorDiff };

            LabelInfo.Content = String.Format("X={0:D4}, Y={1:D4}", x, y);

            for (int i = 0; i < 3; ++i )
            {
                if (bitmapInfos[i] == null) continue;

                System.Drawing.Color color = bitmapInfos[i].GetPixelColor(x, y);
                labels[i].Content = String.Format("A={0:D3}, R={1:D3}, G={2:D3}, B={3:D3}", 
                    color.A, color.R, color.G, color.B);
            }
        }

    }



}
