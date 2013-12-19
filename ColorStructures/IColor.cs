using System.Drawing;

namespace VisualImageDiff.ColorStructures
{
    public interface IColor
    {
        // Get value of components in their original range [0, 255] or [0, 65535]
        double A { get; }
        double R { get; }
        double G { get; }
        double B { get; }

        // Get value of components in the range [0, 1]
        double ANormalized { get; }
        double RNormalized { get; }
        double GNormalized { get; }
        double BNormalized { get; }

        Color ToMsdnColor();
    }
}
