using System;

namespace MathModeling.Rv
{
    public interface IRvGenerator : IValueGenerator
    {
        string Name { get; }

        Func<double, double> DistributionFunction { get; }

        double LowerRarefiedBound { get; }

        double UpperRarefiedBound { get; }
    }
}