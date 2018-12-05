using System;
using System.Linq;

namespace MathModeling.Utils
{
    public static class Algorithm
    {
        public static double BinaryArgumentSearch(this Func<double, double> increasingFunc, double expectedResult,
            double from = double.MinValue, double to = double.MinValue, double argumentEps = 0.01)
        {
            while (to - from > argumentEps * 10)
            {
                var mid = (from + to) / 2;

                var result = increasingFunc(mid);

                if (result < expectedResult)
                {
                    from = mid;
                }
                else
                {
                    to = mid;
                }
            }

            var rangeSteps = (int) Math.Ceiling((to - from) / argumentEps);

            return Enumerable.Range(-10, 20 + rangeSteps)
                .Select(x => from + x * argumentEps)
                .Select(x => (argument: x, residual: Math.Abs(increasingFunc(x) - expectedResult)))
                .OrderBy(x => x.residual)
                .Select(x => x.argument)
                .First();
        }
    }
}