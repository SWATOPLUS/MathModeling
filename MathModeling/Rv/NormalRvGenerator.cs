using System;
using System.Linq;
using Lab2Deposit.Utils;
using MathModeling.Brv;
using Meta.Numerics.Functions;

namespace MathModeling.Rv
{
    public class NormalRvGenerator : IRvGenerator
    {
        public string Name { get; } = "Normal";

        public Func<double, double> DistributionFunction { get; }
        public double LowerRarefiedBound { get; }
        public double UpperRarefiedBound { get; }

        public double Mean { get; }
        public double Variance { get; }

        private readonly IBrvSensor _sensor;

        public NormalRvGenerator(IBrvSensor sensor, double mean, double variance)
        {
            _sensor = sensor;
            Mean = mean;
            Variance = variance;
            DistributionFunction = x => 0.5 * (1 + AdvancedMath.Erf((x - Mean) / Math.Sqrt(2 * Variance)));
            LowerRarefiedBound = DistributionFunction
                .BinaryArgumentSearch(RvUtils.RarefiedBoundEps / 2, double.MinValue, mean);
            UpperRarefiedBound = Mean + (Mean - LowerRarefiedBound);
        }

        public double Generate()
        {
            var r = Enumerable.Range(0, 12)
                .Select(x => _sensor.Generate())
                .Sum();

            return Mean + (r - 6) * Math.Sqrt(Variance);
        }
    }
}