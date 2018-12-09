using MathModeling.Rv;
using System;
using System.Linq;

namespace MathModeling.Markov
{
    public class MarkovGenerator : IValueGenerator
    {
        private readonly IMarkovChainFactory _factory;
        private readonly double[] _values;
        private readonly int _step;
        private readonly Func<int, double> _trend;

        public MarkovGenerator(IMarkovChainFactory factory, double[] values, int step, Func<int, double> trend = null)
        {
            _factory = factory;
            _values = values;
            _step = step;
            _trend = trend ?? (x => 0);
        }

        public double Generate()
        {
            var chain = _factory.Create();

            foreach (var x in Enumerable.Range(0, _step))
            {
                chain.NextState();
            }

            return _trend(_step) + _values[chain.State];
        }
    }
}
