using System;
using MathModeling.Brv;

namespace MathModeling.Rv
{
    public class UniformRvGenerator : IRvGenerator
    {
        public string Name { get; } = "Uniform";

        public Func<double, double> DistributionFunction { get; }
        public double LowerRarefiedBound { get; }
        public double UpperRarefiedBound { get; }

        public double Mean { get; }
        public double Variance { get; }

        private readonly IBrvSensor _sensor;
        private readonly double _a;
        private readonly double _b;

        public UniformRvGenerator(IBrvSensor sensor, double a, double b)
        {
            _sensor = sensor;
            _a = a;
            _b = b;
            Mean = (a + b) / 2;
            Variance = (b - a) * (b - a) / 12;
            LowerRarefiedBound = a;
            UpperRarefiedBound = b;

            DistributionFunction = x =>
            {
                if (x <= a)
                {
                    return 0;
                }

                if (x >= b)
                {
                    return 1;
                }

                return (x - a) / (b - a);
            };

        }

        public double Generate()
        {
            var eta = _sensor.Generate();

            return _a + (_b - _a) * eta;
        }
    }
}