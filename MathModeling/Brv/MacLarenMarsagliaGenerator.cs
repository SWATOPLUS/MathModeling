using System;
using System.Linq;

namespace MathModeling.Brv
{
    public class MacLarenMarsagliaGenerator : IBrvSensor
    {
        private const int K = 1000; // size of helping table
        private readonly double[] _v; // helping table
        private readonly IBrvSensor _b;// _b and _c are independent BRV - sensors 
        private readonly IBrvSensor _c; //
        public MacLarenMarsagliaGenerator(IBrvSensor b, IBrvSensor c)
        {
            if (ReferenceEquals(b, c))
            {
                throw new ArgumentException($"{nameof(b)} and {nameof(c)} BRV sensors must be different");
            }

            _b = b;
            _c = c;
            _v = Enumerable.Range(0, K)
                .Select(x => _b.Generate())
                .ToArray();
        }

        public double Generate()
        {
            var s = (int)Math.Floor(_c.Generate() * K); // get index of item from helping table
            var a = _v[s]; // get result item
            _v[s] = _b.Generate(); // replace result item by new
            return a;
        }
    }
}