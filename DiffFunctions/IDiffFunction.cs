using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualImageDiff.DiffFunctions
{
    public interface IDiffFunction
    {
        String Name { get; }
        BitmapInfo CreateDiff(BitmapInfo left, BitmapInfo right);
    }
}
