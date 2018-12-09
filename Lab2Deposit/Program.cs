using System;
using System.Collections.Generic;
using System.Linq;
using MathModeling.Brv;
using MathModeling.Markov;
using MathModeling.Rv;
using MoreLinq;

namespace Lab2Deposit
{
    internal class Program
    {
        private static readonly double[,] MarkovSettings = {
            { 0.2, 0.7, 0.1 },
            { 0.2, 0.5, 0.3 },
            { 0.2, 0.6, 0.2 }
        };

        private static readonly double[] MarkovValues = { -1.0, 0.0, 2.0 };

        private static void Main(string[] args)
        {
            var sensor = new MacLarenMarsagliaGenerator(
                new LinearCongruentialGenerator(3),
                new LinearCongruentialGenerator(5));

            var uniformGenerator = new UniformRvGenerator(sensor, 1840.0, 2000.0);
            var normalGenerator = new NormalRvGenerator(sensor, 1965, 0.1);
            var discreteGenerator = new DiscreteRvGenerator(sensor,
                new[] { (1840.0, 0.13), (1850, 0.47), (1970.0, 0.12), (1980.0, 0.28) });
            TestGenerator(uniformGenerator);
            TestGenerator(normalGenerator);
            TestGenerator(discreteGenerator);

            var factory = new MarkovChainFactory
            (
                sensor,
                MarkovSettings,
                1
            );
            var markovGenerator = new MarkovGenerator(factory, MarkovValues, 90, x => 1830.0 + x);

            SolveTask(discreteGenerator, discreteGenerator.Name);
            SolveTask(uniformGenerator, uniformGenerator.Name);
            SolveTask(normalGenerator, normalGenerator.Name);
            SolveTask(markovGenerator, "Markov");
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

        private static void SolveTask(IValueGenerator generator, string name)
        {
            var amount = 1000;
            var values = generator.GetValues(amount);

            var p = new double[3];

            foreach (var x in values)
            {
                var decision = GetDecision(x);
                p[decision - 1] += 1;
            }

            Console.WriteLine($"Generator is {name}, result p1: {p[0] / amount}, p2: {p[1] / amount}, p3: {p[2] / amount}");
        }

        private static int GetDecision(double k1)
        {
            var k0 = 1820.0;
            var annualDollarRate = 0.1;
            var annualRoubleRate = 0.5;

            var k1Star = k0 * (1 + annualDollarRate);
            var k1Up = k1Star / (1 + annualRoubleRate);

            if (k1Star < k1)
            {
                return 1;
            }

            if (k1Up < k1)
            {
                return 2;
            }


            return 3;
        }
    }
}
