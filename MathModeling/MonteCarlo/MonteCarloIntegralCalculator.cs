using System;
using System.Collections.Generic;
using System.Linq;

namespace MathModeling.MonteCarlo
{
    public class MonteCarloIntegralCalculator
    {
        private static readonly Random Random = new Random(0);

        public double From { get; }

        public double To { get; }

        public Func<double, double> IntegralFunction { get; }

        public MonteCarloIntegralCalculator(double from, double to, Func<double, double> integralFunction)
        {
            From = from;
            To = to;
            IntegralFunction = integralFunction;
        }

        private static IEnumerable<double> GetUniformRandomValues(int n, double a, double b)
        {
            return Enumerable.Range(0, n)
                .Select(x => Random.NextDouble())
                .Select(x => a + (b - a) * x);
        }

        public double CalculateIntegral(int n)
        {
            var values = GetUniformRandomValues(n, From, To)
                .Select(IntegralFunction);

            return (To - From) * values.Average();
        }
    }
}
