using System;
using System.Linq;
using MathModeling.Brv;
using MathModeling.Rv;

namespace Lab1Exp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var n = 1000; // amount of numbers
            var lambdas = new[] {0.5, 1.0, 1.5}; //lambda-parameter for generator
            var gridSize = 10; //size of the grid for Chi-squared test

            var sensor = new MacLarenMarsagliaGenerator(
                new LinearCongruentialGenerator(3),
                new LinearCongruentialGenerator(5));

            foreach (var lambda in lambdas)
            {
                var generator = new ExponentialGenerator(sensor, lambda);

                var values = Enumerable.Range(0, n)
                    .Select(x => generator.Generate())
                    .ToArray();

                Console.WriteLine($"Lambda is {generator.Lambda}");
                var chi2 = RvUtils.ChiSquaredTest(values, generator.DistributionFunction, gridSize,
                    generator.LowerRarefiedBound, generator.UpperRarefiedBound);
                Console.WriteLine($"Chi^2 is {chi2}");
                Console.WriteLine($"Degrees of freedom is {gridSize - 1}");
            }
        }
    }
}
