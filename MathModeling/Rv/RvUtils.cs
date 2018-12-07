using System;
using System.Collections.Generic;
using System.Linq;

namespace MathModeling.Rv
{
    public static class RvUtils
    {
        public const double RarefiedBoundEps = 0.01;

        public static double ChiSquaredTest(IReadOnlyCollection<double> values, Func<double, double> distributionFunc,
            int gridSize, double lowerBound, double upperBound)
        {
            var snippetLength = (upperBound - lowerBound) / (gridSize - 1);

            int GetSectionNumber(double x)
            {
                if (x <= lowerBound || x >= upperBound)
                {
                    return gridSize - 1;
                }

                return (int) Math.Floor((x - lowerBound) / snippetLength);
            }

            var probabilities = Enumerable.Range(0, gridSize - 1)
                .Select(x => distributionFunc(lowerBound + (x + 1) * snippetLength) -
                             distributionFunc(lowerBound + x * snippetLength))
                // todo: do we really need this?
                //.Append(1 - distributionFunc(upperBound) + distributionFunc(lowerBound))
                .ToArray();
 
            var frequencies = new double[gridSize];
            values
                .Select(GetSectionNumber)
                .GroupBy(x => x)
                .ToList()
                .ForEach(x => frequencies[x.Key] = (double) x.Count() / values.Count);

            return CalcChiSquare(frequencies, probabilities) * values.Count;
        }

        private static double CalcChiSquare(IList<double> practical, IList<double> theoretical)
        {
            var len = Math.Min(practical.Count, theoretical.Count);
            var value = 0.0;
            for (var i = 0; i < len; i++)
            {
                if (theoretical[i] < 1e-6 && practical[i] < 1e-6)
                {
                    continue;
                }

                value += Math.Pow(practical[i] - theoretical[i], 2) / theoretical[i];
            }

            return value;
        }
    }

}
