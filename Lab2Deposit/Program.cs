using System;
using System.Collections.Generic;
using System.Linq;
using MathModeling.Brv;
using MathModeling.Rv;
using MoreLinq;

namespace Lab2Deposit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sensor = new MacLarenMarsagliaGenerator(
                new LinearCongruentialGenerator(3),
                new LinearCongruentialGenerator(5));

            var uniformGenerator = new UniformRvGenerator(sensor, 1840.0, 2000.0);
            var normalGenerator = new NormalRvGenerator(sensor, 1965, 0.1);
            var discreteGenerator = new DiscreteRvGenerator(sensor,
                new[] {(1840.0, 0.13), (1850, 0.47), (1970.0, 0.12), (1980.0, 0.28)});
            TestGenerator(uniformGenerator);
            TestGenerator(normalGenerator);
            TestGenerator(discreteGenerator);
        }

        private static void TestGenerator(IRvGenerator generator)
        {
            var n = 1000; // amount of numbers
            var gridSize = 20; //size of the grid for Chi-squared test
            var values = Enumerable.Range(0, n).Select(x => generator.Generate()).ToArray();
            var chi2 = RvUtils.ChiSquaredTest(values, generator.DistributionFunction, gridSize,
                generator.LowerRarefiedBound, generator.UpperRarefiedBound);

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Generator is {generator.Name}");
            Console.WriteLine($"Chi^2 is {chi2}");
            Console.WriteLine($"Degrees of freedom is {gridSize - 1}");
            Console.WriteLine("-------------------------------------------");
        }
    }
}
