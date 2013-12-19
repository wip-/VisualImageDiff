using System;
using VisualImageDiff.ColorStructures;

namespace VisualImageDiff.Functions.ColorFunctions
{
    /// <summary>
    /// Interface for functions that return an int
    /// Note: could use generic prototype stuff (TODO check how to do in C#)
    /// </summary>
    public abstract class BaseColorFunction
    {
        public abstract String Name { get; }
        public abstract double yInputMin { get; }
        public abstract double yInputMax { get; }

        public abstract double GetYvalue(IColor color);
    }
}
