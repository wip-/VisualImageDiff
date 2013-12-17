using System;
using System.Drawing;

namespace VisualImageDiff.Functions.ColorFunctions
{
    /// <summary>
    /// Interface for functions that return an int
    /// Note: could use generic prototype stuff (TODO check how to do in C#)
    /// </summary>
    public interface IColorFunction
    {
        String Name { get; }
        int GetYvalue(Color color, int yMax);
    }
}
