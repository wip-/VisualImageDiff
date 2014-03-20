using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using VisualImageDiff.ColorStructures;
using VisualImageDiff.DiffFunctions;
using VisualImageDiff.Functions;
using VisualImageDiff.Functions.ColorFunctions;
using VisualImageDiff.ImageFunctions;




namespace VisualImageDiff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap bitmapLeft;
        Bitmap bitmapRight;
        Bitmap bitmapDiffLeft;
        Bitmap bitmapDiffRight;

        BitmapInfo bitmapInfoLeft;
        BitmapInfo bitmapInfoRight;
        BitmapInfo bitmapInfoDiffLeft;
        BitmapInfo bitmapInfoDiffRight;

        String imageSourceFileNameFirst;

        ConnectionViewModel connectionViewModel = new ConnectionViewModel();


        private void HardCodedQuickBatchMode1()
        {
            String errorMessage;
            String[] files = Directory.GetFiles(
                ".",
                "*.png", SearchOption.TopDirectoryOnly);
            Bitmap original = LoadBitmap(
                "myImage.png",
                out errorMessage);
            BitmapInfo originalData = new BitmapInfo(original);

            List<CachedDiffFunction> curveFunctions = new List<CachedDiffFunction>();
            curveFunctions.Add(new MsdnHsbHDiffGrayscale());

            List<String> prefixes = new List<String>();
            prefixes.Add("-what");

            foreach (var file in files)
            {
                Bitmap toCompare = LoadBitmap(file, out errorMessage);
                BitmapInfo dataToCompare = new BitmapInfo(toCompare);

                for (int i = 0; i < curveFunctions.Count; ++i)
                {
                    BitmapInfo diff = curveFunctions[i].CreateDiff(
                        originalData, dataToCompare, CachedDiffFunction.EnableCache.False);
                    String newFileName = AddToFileName(file, prefixes[i]);
                    diff.ToBitmap().Save(newFileName);
                }
            }
        }


        private void HardCodedQuickBatchMode2()
        {
            String errorMessage;
            String folderRoot = ".\\";
            String[] files = Directory.GetFiles(folderRoot + "outputs", "*.png", SearchOption.TopDirectoryOnly);
            Bitmap original = LoadBitmap(folderRoot + "myImage.png", out errorMessage);
            BitmapInfo originalData = new BitmapInfo(original);

            List<IDiffFunction> curveFunctions = new List<IDiffFunction>();
            curveFunctions.Add(new DualProcess<CurveImageFunction<RgbRComponent>>());

            List<String> folders = new List<String>();
            folders.Add("reds\\");

            for (int i = 0; i < folders.Count; ++i)
            {
                if (!Directory.Exists(folderRoot + folders[i]))
                    Directory.CreateDirectory(folderRoot + folders[i]);
            }
            
            foreach (var file in files)
            {
                Bitmap toCompare = LoadBitmap(file, out errorMessage);
                BitmapInfo dataToCompare = new BitmapInfo(toCompare);

                for (int i = 0; i < curveFunctions.Count; ++i)
                {
                    CachedDiffFunction cachedDiff = curveFunctions[i] as CachedDiffFunction;
                    if (cachedDiff == null)
                        continue;

                    BitmapInfo diff = cachedDiff.CreateDiff(originalData, dataToCompare, CachedDiffFunction.EnableCache.False);
                    String newFileName = folderRoot + folders[i] + Path.GetFileNameWithoutExtension(file) + ".png";
                    diff.ToBitmap().Save(newFileName, ImageFormat.Png);
                }
            }
        }


        private void HardCodedQuickBatchMode3()
        {
            String errorMessage;
            String folderRoot = ".\\";
            String[] files = Directory.GetFiles(folderRoot + "outputs", "*.png", SearchOption.TopDirectoryOnly);
            Bitmap original = LoadBitmap(folderRoot + "hueGradation.png", out errorMessage);
            BitmapInfo originalData = new BitmapInfo(original);

            var diffFunction = new DualProcess<CurveImageFunction<RgbGComponent>>();
            String csvFileName = folderRoot + "greens.csv";
            List<double[]> columns = new List<double[]>();

            //double[] firstColumn = new double[original.Width];
            //for(int x = 0; x<original.Width; ++x)
            //    firstColumn[x] = x;
            //columns.Add(firstColumn);

            foreach (var file in files)
            {
                Bitmap toCompare = LoadBitmap(file, out errorMessage);
                BitmapInfo dataToCompare = new BitmapInfo(toCompare);
                diffFunction.CreateDiff(originalData, dataToCompare, CachedDualDiffFunction.EnableCache.False);
                ICurveFunction curveFunction = diffFunction.RightImageFunction;
                columns.Add(curveFunction.CurveValues);
            }

            using (StreamWriter file = new StreamWriter(csvFileName))
            {
                // chart corner
                file.Write("Hue");

                // chart headers
                for(int c = 0; c< columns.Count; ++c)
                    file.Write(", Density {0}%", c);

                // chart contents
                for (int x = 0; x < original.Width; ++x)
                {
                    // first column: hue
                    file.Write("\n{0}", x);

                    // next columns: component value
                    for (int c = 0; c < columns.Count; ++c)
                        file.Write(", {0}", columns[c][x]);
                }
            }
            Process.Start("explorer.exe", @"/select,""" + csvFileName + "\"");
        }



        private void HardCodedQuickBatchMode4()
        {
            String errorMessage;
            String folderRoot = @".\";
            String[] files = Directory.GetFiles(folderRoot + @"images-in\", "*.png", SearchOption.TopDirectoryOnly);
            Bitmap original = LoadBitmap(folderRoot + "refImage.png", out errorMessage);
            BitmapInfo originalData = new BitmapInfo(original);

            IImageFunction curveFunction = new CurveImageFunction<MsdnHsbHComponent>();

            foreach (var file in files)
            {
                Bitmap toLoad = LoadBitmap(file, out errorMessage);
                BitmapInfo dataToLoad = new BitmapInfo(toLoad);
                BitmapInfo dataCurve = new BitmapInfo(toLoad);

                curveFunction.FillImage(dataToLoad, dataCurve);
                String newFileName = folderRoot + @"curves-out\" + Path.GetFileNameWithoutExtension(file) + ".png";
                dataCurve.ToBitmap().Save(newFileName, ImageFormat.Png);
            }
        }


        public MainWindow()
        {
            //HardCodedQuickBatchMode4();

            InitializeComponent();
            DataContext = connectionViewModel;
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

            // TODO use async / await

            if (msg != null)
            {
                LabelInfo.Content = msg;
            }
            else
            {
                bitmapInfoLeft = new BitmapInfo(bitmapLeft);
                connectionViewModel.InvalidateDiffCaches();
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
                connectionViewModel.InvalidateDiffCaches();
                CreateDiff();
            }
        }

        private void SliderParam_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CreateDiff();

            // refresh scrollviewer zooms
            Zoom(GetCurrentZoom());
        }

        private void ComboBoxDiffFunction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateDiff();

            // refresh scrollviewer zooms
            Zoom(GetCurrentZoom());
        }

        private void CreateDiff()
        {
            if (bitmapLeft != null && bitmapRight != null)
            {
                ISimpleDiffFunction simpleDiff = connectionViewModel.SelectedDiffFunction as ISimpleDiffFunction;
                if (simpleDiff != null)
                {
                    IParamFunction paramFunction = simpleDiff as IParamFunction;
                    if (paramFunction != null)
                        paramFunction.Parameter = SliderParam.Value;


                    bitmapInfoDiffLeft = simpleDiff.CreateDiff(bitmapInfoLeft, bitmapInfoRight);

                    bitmapDiffLeft = bitmapInfoDiffLeft.ToBitmap();
                    ImageDiffLeft.Source =
                        Imaging.CreateBitmapSourceFromHBitmap(
                        bitmapDiffLeft.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    ImageDiffRight.Source = null;
                }

                IDualDiffFunction dualDiff = connectionViewModel.SelectedDiffFunction as IDualDiffFunction;
                if (dualDiff != null)
                {
                    BitmapInfoPair diffs = dualDiff.CreateDiff(bitmapInfoLeft, bitmapInfoRight);

                    bitmapInfoDiffLeft = diffs.left;
                    bitmapDiffLeft = bitmapInfoDiffLeft.ToBitmap();
                    ImageDiffLeft.Source =
                        Imaging.CreateBitmapSourceFromHBitmap(
                        bitmapDiffLeft.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    bitmapInfoDiffRight = diffs.right;
                    bitmapDiffRight = bitmapInfoDiffRight.ToBitmap();
                    ImageDiffRight.Source =
                        Imaging.CreateBitmapSourceFromHBitmap(
                        bitmapDiffRight.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
            }
        }



        private static Bitmap LoadBitmap(String filename, out String errorMessage)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                if (fs != null)
                    fs.Close();
                errorMessage = "File already in use!";
                return null;
            }

            Bitmap bitmap;
            //try
            {
                bitmap = new Bitmap(fs);
                errorMessage = null;
            }
            //catch (System.Exception /*ex*/)
            //{
            //    bitmap.Dispose();
            //    errorMessage = "Not an image!";
            //}
            return bitmap;
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
                if (imageSourceFileNameFirst == null)
                    imageSourceFileNameFirst = imageSourceFileName;

                if (!File.Exists(imageSourceFileName))
                    return "Not a file!";

                String errorMessage;
                destinationBitmap = LoadBitmap(imageSourceFileName, out errorMessage);
                if (errorMessage != null)
                    return errorMessage;

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
            {
                ScrollViewerImageLeft, ScrollViewerImageRight, 
                ScrollViewerImageDiffLeft, ScrollViewerImageDiffRight
            };

            foreach (ScrollViewer scrollViewer in scrollViewers )
            {
                if (scrollViewer == null)
                    continue;
                scrollViewer.ScrollToVerticalOffset(vert);
                scrollViewer.ScrollToHorizontalOffset(hori);
                scrollViewer.UpdateLayout();
            }
        }

        private void SliderZoomOut_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           if(SliderZoomOut.Value!=1)
               Zoom(SliderZoomOut.Value);
           if (SliderZoomIn!=null)
            SliderZoomIn.Value = 1;
        }

        private void SliderZoomIn_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SliderZoomIn.Value != 1)
                Zoom(SliderZoomIn.Value);
            if (SliderZoomOut != null)
            SliderZoomOut.Value = 1;
        }

        private void ButtonResetZoom_Click(object sender, RoutedEventArgs e)
        {
            Zoom(1);
            SliderZoomIn.Value = 1;
            SliderZoomOut.Value = 1;
        }

        private double GetCurrentZoom()
        {
            if (SliderZoomIn == null || SliderZoomOut == null)
                return 1;

            if (SliderZoomIn.Value != 1)
                return SliderZoomIn.Value;
            if (SliderZoomOut.Value!=1)
                return SliderZoomIn.Value;
            return 1;
        }

        private void Zoom(double val)
        {
            try
            {
                ScaleTransform myScaleTransform = new ScaleTransform();
                myScaleTransform.ScaleY = val;
                myScaleTransform.ScaleX = val;
                if (LabelZoom != null)
                    LabelZoom.Content = val;
                TransformGroup myTransformGroup = new TransformGroup();
                myTransformGroup.Children.Add(myScaleTransform);

                System.Windows.Controls.Image[] images = new System.Windows.Controls.Image[] 
                { 
                    ImageLeft, ImageRight, 
                    ImageDiffLeft, ImageDiffRight 
                };

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






        private void ImageLeft_MouseMove(object sender, MouseEventArgs e)
        {
            Image_MouseMove(ImageLeft, e);
        }

        private void ImageRight_MouseMove(object sender, MouseEventArgs e)
        {
            Image_MouseMove(ImageRight, e);
        }

        private void ImageDiffLeft_MouseMove(object sender, MouseEventArgs e)
        {
            Image_MouseMove(ImageDiffLeft, e);
        }

        private void ImageDiffRight_MouseMove(object sender, MouseEventArgs e)
        {
            Image_MouseMove(ImageDiffRight, e);
        }

        private void Image_MouseMove(System.Windows.Controls.Image clickedImage, MouseEventArgs e)
        {
            int x = (int)(e.GetPosition(clickedImage).X);
            int y = (int)(e.GetPosition(clickedImage).Y);

            BitmapInfo[] bitmapInfos = new BitmapInfo[] 
            { 
                bitmapInfoLeft, bitmapInfoRight, 
                bitmapInfoDiffLeft, bitmapInfoDiffRight, 
            };

            System.Windows.Controls.Label[] labels = new System.Windows.Controls.Label[] 
            { 
                LabelColorLeft, LabelColorRight, 
                LabelColorDiffLeft, LabelColorDiffRight
            };

            LabelInfo.Content = String.Format("X={0:D4}, Y={1:D4}", x, y);

            for (int i = 0; i < 4; ++i )
            {
                if (bitmapInfos[i] == null) continue;

                IColor color = bitmapInfos[i].GetPixelColor(x, y);
                labels[i].Content = Helpers.ColorConversion.GetColorsString(color);
            }
        }


        private void ButtonSaveResultLeft_Click(object sender, RoutedEventArgs e)
        {
            ButtonSaveResultImage(bitmapDiffLeft);
        }

        private void ButtonSaveResultRight_Click(object sender, RoutedEventArgs e)
        {
            ButtonSaveResultImage(bitmapDiffRight);
        }

        private void ButtonSaveResultImage(Bitmap bitmap)
        {
            if (bitmap == null)
                return;

            SaveFileDialog dialogSaveFile = new SaveFileDialog();
            dialogSaveFile.Filter = "Supported images|*.png";
            dialogSaveFile.InitialDirectory = Path.GetDirectoryName(imageSourceFileNameFirst);
            dialogSaveFile.FileName = AddToFileName(imageSourceFileNameFirst, "-diff");

            if ((bool)dialogSaveFile.ShowDialog())
            {
                Stream saveStream;
                if ((saveStream = dialogSaveFile.OpenFile()) != null)
                {
                    bitmap.Save(saveStream, ImageFormat.Png);
                    saveStream.Close();
                }
            }
        }


        String AddToFileName(String filename, String addChars)
        {
            return Path.GetFileNameWithoutExtension(filename) + addChars + Path.GetExtension(filename);
        }

        private void ButtonSaveCurveValuesLeft_Click(object sender, RoutedEventArgs e)
        {
            ButtonSaveCurveValues(
                (IDiffFunction diffFunction) => (connectionViewModel.SelectedDiffFunction as dynamic).LeftImageFunction );
        }

        private void ButtonSaveCurveValuesRight_Click(object sender, RoutedEventArgs e)
        {
            ButtonSaveCurveValues(
                (IDiffFunction diffFunction) => (connectionViewModel.SelectedDiffFunction as dynamic).RightImageFunction);
        }

        private delegate ICurveFunction CurveFunctionSelector(IDiffFunction diffFunction);

        private void ButtonSaveCurveValues(CurveFunctionSelector curveFunctionSelector)
        {
            Type selectionType = connectionViewModel.SelectedDiffFunction.GetType();
            if (selectionType.IsGenericType)
            {
                if (selectionType.GetGenericTypeDefinition() == typeof(DualProcess<>))
                {
                    // SelectedDiffFunction is a DualProcess<T>

                    foreach (Type typeArgument in selectionType.GetGenericArguments())
                    {
                        if (typeArgument.GetInterfaces().Contains(typeof(ICurveFunction)))
                        {
                            // argument T in DualProcess<T> inherits from ICurveFunction

                            ICurveFunction curveFunction = curveFunctionSelector(connectionViewModel.SelectedDiffFunction);
                            SaveToCsv(curveFunction.CurveValues);
                            return;
                        }
                    }
                }
            }
        }

        private void SaveToCsv(double[] values)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = Path.GetDirectoryName(imageSourceFileNameFirst);
            saveDialog.FileName = Path.GetFileNameWithoutExtension(imageSourceFileNameFirst) + "-curve";
            saveDialog.Filter = "comma-separated values (.csv)|*.csv";
            saveDialog.DefaultExt = ".csv";
            
            if (saveDialog.ShowDialog().Value)
            {
                using (StreamWriter file = new StreamWriter(saveDialog.FileName))
                {
                    foreach (double value in values)
                        file.WriteLine(value);
                }
                Process.Start("explorer.exe", @"/select,""" + saveDialog.FileName + "\"");
            }
        }


    }



}
