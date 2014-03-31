using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VisualImageDiff.DiffFunctions;
using VisualImageDiff.Functions.ColorFunctions;
using VisualImageDiff.ImageFunctions;

namespace VisualImageDiff
{
    /// <summary>
    /// Wraps DiffFunctions list
    /// </summary>
    public class ConnectionViewModel : ViewModelBase
    {
        public ConnectionViewModel()
        {
            diffFunctions = new List<IDiffFunction>();

            diffFunctions.Add(new Prototype());
            diffFunctions.Add(new Blend());

            diffFunctions.Add(new ColorDiffFunction<RgbRComponent>());
            diffFunctions.Add(new ColorDiffFunction<RgbGComponent>());
            diffFunctions.Add(new ColorDiffFunction<RgbBComponent>());

            diffFunctions.Add(new ColorDiffFunction<MsdnHsbHComponent>());
            diffFunctions.Add(new ColorDiffFunction<MsdnHsbSComponent>());
            diffFunctions.Add(new ColorDiffFunction<MsdnHsbBComponent>());

            diffFunctions.Add(new MsdnHsbHDiffGrayscale());
            diffFunctions.Add(new MsdnHsbSDiffGrayscale());

            diffFunctions.Add(new RedRatioGrayscale());
            diffFunctions.Add(new LuminanceGrayscaleLeft());
            diffFunctions.Add(new LuminanceGrayscaleRight());
            diffFunctions.Add(new LuminanceGrayscaleDiff());

            diffFunctions.Add(new DualProcess<CurveImageFunction<RgbRComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<RgbGComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<RgbBComponent>>());

            diffFunctions.Add(new DualProcess<CurveImageFunction<MsdnHsbHComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<MsdnHsbSComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<MsdnHsbBComponent>>());

            diffFunctions.Add(new DualProcess<CurveImageFunction<CieXyzXComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<CieXyzYComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<CieXyzZComponent>>());

            diffFunctions.Add(new DualProcess<CurveImageFunction<CieLabLComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<CieLabAComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<CieLabBComponent>>());

            diffFunctions.Add(new DualProcess<CurveImageFunction<YuvYComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<YuvUComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<YuvVComponent>>());

            diffFunctions.Add(new DualProcess<CurveImageFunction<AdobeRgbRComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<AdobeRgbGComponent>>());
            diffFunctions.Add(new DualProcess<CurveImageFunction<AdobeRgbBComponent>>());

            diffFunctions.Add(new DualProcess<DerivativeCurveImageFunction<RgbRComponent>>());
            diffFunctions.Add(new DualProcess<DerivativeCurveImageFunction<RgbGComponent>>());
            diffFunctions.Add(new DualProcess<DerivativeCurveImageFunction<RgbBComponent>>());

            diffFunctions.Add(new DualProcess<DerivativeCurveImageFunction<MsdnHsbHComponent>>());

            //diffFunctions.Add(new HsvColorHDiffCurve());
            //diffFunctions.Add(new HsvColorSDiffCurve());
            //diffFunctions.Add(new HsvColorVDiffCurve());
            //diffFunctions.Add(new RedRatioCurve());

            SelectedDiffFunction = diffFunctions[0];
        }

        private readonly IList<IDiffFunction> diffFunctions;
        public IEnumerable<IDiffFunction> DiffFunctions { get { return diffFunctions; } }

        private IDiffFunction selectedDiffFunction;
        public IDiffFunction SelectedDiffFunction
        {
            get { return selectedDiffFunction; }
            set { SetValue(ref selectedDiffFunction, value); }
        }

        /// <summary>
        /// Invalidate caches of diff functions implementing one
        /// </summary>
        public void InvalidateDiffCaches()
        {
            foreach (IDiffFunction diff in diffFunctions)
            {
                CachedDiffFunction cDiff = diff as CachedDiffFunction;
                if (cDiff != null)
                    cDiff.InvalidateCache();
            }
        }
    }




    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
