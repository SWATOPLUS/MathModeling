using System;
using System.Collections.Generic;
using System.Linq;
using MathModeling.Brv;

namespace MathModeling.Rv
{
    public class DiscreteRvGenerator : IRvGenerator
    {
        private readonly IBrvSensor _sensor;
        public string Name { get; } = "Discrete";
        public Func<double, double> DistributionFunction { get; }
        public double LowerRarefiedBound { get; }
        public double UpperRarefiedBound { get; }

        private (double value, double chance, double cumulant)[] Chances { get; }

        public DiscreteRvGenerator(IBrvSensor sensor, IEnumerable<(double value, double chance)> chances)
        {
            _sensor = sensor;

            Chances = CalcCumulant(chances);

            LowerRarefiedBound = Chances.First().value - 1.0;
            UpperRarefiedBound = Chances.Last().value + 1.0;
            DistributionFunction = x =>
            {
                foreach (var (value, _, cumulant) in Chances.Reverse())
                {
                    if (value <= x)
                    {
                        return cumulant;
                    }
                }

                return 0.0;
            };
        }

        private static (double value, double chance, double cumulant)[] CalcCumulant(IEnumerable<(double value, double chance)> chances)
        {
            var cumulant = 0.0;

            var list = new List<(double value, double chance, double cumulant)>();

            foreach (var (value, chance) in chances.OrderBy(x => x.value))
            {
                cumulant += chance;
                list.Add((value, chance, cumulant));
            }

            return list.ToArray();
        }

        public double Generate()
        {
            var eta = _sensor.Generate();

            //todo: use binary search to improve performance
            foreach (var x in Chances)
            {
                if (eta < x.cumulant)
                {
                    return x.value;
                }
            }

            return Chances.Last().value;
        }
    }
}
