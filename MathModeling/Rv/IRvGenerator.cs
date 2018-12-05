using System;

namespace MathModeling.Rv
{
    public interface IRvGenerator
    {
        string Name { get; }

        Func<double, double> DistributionFunction { get; }

        double LowerRarefiedBound { get; }

        double UpperRarefiedBound { get; }

        double Generate();
    }
}