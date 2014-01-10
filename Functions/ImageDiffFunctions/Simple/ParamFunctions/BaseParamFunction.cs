using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualImageDiff.DiffFunctions
{
    /// <summary>
    /// Functions that take an additional input parameter
    /// </summary>
    public interface IParamFunction : ISimpleDiffFunction
    {
        double Parameter { get; set; }
        BitmapInfo CreateDiff(BitmapInfo left, BitmapInfo right);
    }
}
