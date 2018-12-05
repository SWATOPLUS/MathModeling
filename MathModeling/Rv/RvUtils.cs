using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace MathModeling.Rv
{
    public static class RvUtils
    {
        public const double RarefiedBoundEps = 0.01;

        public static double ChiSquaredTest(IReadOnlyCollection<double> values, Func<double, double> distributionFunc, int gridSize)
        {
            int GetSectionNumber(double x)
            {
                if (x >= gridSize - 1)
                {
                    return gridSize - 1;
                }

                return (int)x;
            }

            var probabilities = new double[gridSize];
            foreach (var section in Enumerable.Range(0, gridSize - 1))
            {
                probabilities[section] = distributionFunc(section + 1) -
                                         distributionFunc(section);
            }
            probabilities[gridSize - 1] = 1 - distributionFunc(gridSize - 1);

            var frequencies = new double[gridSize];
            values
                .Select(GetSectionNumber)
                .GroupBy(x => x)
                .ForEach(x => frequencies[x.Key] = (double)x.Count() / values.Count);

            return CalcChiSquare(frequencies, probabilities) * values.Count;
        }

        private static double CalcChiSquare(IList<double> practical, IList<double> theoretical)
        {
            var len = Math.Min(practical.Count, theoretical.Count);
            var value = 0.0;
            for (var i = 0; i < len; i++)
            {
                value += Math.Pow(practical[i] - theoretical[i], 2) / theoretical[i];
            }

            return value;
        }
    }

}
