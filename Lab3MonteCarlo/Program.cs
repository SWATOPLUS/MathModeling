using System;
using System.Globalization;
using System.Linq;
using MathModeling.MonteCarlo;

namespace Lab3MonteCarlo
{
    class Program
    {
        static Program()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-us");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-us");
        }

        private static void Main()
        {
            var realValue = 1.3463879568;

            var repeats = 1;

            var sampleSizes = new[]
            {
                10, 20, 50,
                100, 200, 500,
                1000, 2000, 5000,
                10000, 20000, 50000,
                100000, 200000, 500000,
                1000000, 2000000, 5000000
            };

            var calculator = new MonteCarloIntegralCalculator(-1, 3, x => Math.Exp(-x * x) * Math.Cos(x));

            Console.WriteLine($"Real value {realValue}");
            Console.WriteLine("N\t  result\terror");

            foreach (var sampleSize in sampleSizes)
            {
                foreach (var _ in Enumerable.Range(0, repeats))
                {
                    var integral = calculator.CalculateIntegral(sampleSize);
                    var error = Math.Abs(integral - realValue);
                    Console.WriteLine($"{sampleSize.ToString().PadRight(10)}{integral:E}\t{error:E}");
                }
            }

        }
    }
}
