using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using VisualImageDiff.DiffFunctions;

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
            diffFunctions.Add(new RgbDiff());

            diffFunctions.Add(new RedDiffCurve());
            diffFunctions.Add(new GreenDiffCurve());
            diffFunctions.Add(new BlueDiffCurve());

            diffFunctions.Add(new MsdnHsbHDiffGrayscale());
            diffFunctions.Add(new MsdnHsbSDiffGrayscale());
            diffFunctions.Add(new MsdnHsbHDiffCurve());
            diffFunctions.Add(new MsdnHsbSDiffCurve());
            diffFunctions.Add(new MsdnHsbBDiffCurve());

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
