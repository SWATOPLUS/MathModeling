using System;
using Lab2Deposit.Utils;
using MathModeling.Brv;

namespace MathModeling.Rv
{
    public class ExponentialGenerator : IRvGenerator
    {
        public string Name { get; } = "Exponential";
        public Func<double, double> DistributionFunction { get; }
        public double LowerRarefiedBound { get; }
        public double UpperRarefiedBound { get; }

        private readonly IBrvSensor _sensor;
        public double Lambda { get; }

        public ExponentialGenerator(IBrvSensor sensor, double lambda)
        {
            _sensor = sensor;
            Lambda = lambda;

            DistributionFunction = x =>
            {
                if (x < 0.0)
                {
                    return 0.0;
                }

                return 1 - Math.Exp(-x * Lambda);
            };

            LowerRarefiedBound = 0.0;
            UpperRarefiedBound = DistributionFunction
                .BinaryArgumentSearch(1 - RvUtils.RarefiedBoundEps / 2, 0, double.MaxValue);
        }

        public double Generate()
        {
            var eta = _sensor.Generate(); //BRV-parameter

            return -Math.Log(1 - eta) / Lambda;
        }
    }
}