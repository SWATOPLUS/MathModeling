namespace MathModeling.Brv
{
    public class LinearCongruentialGenerator : IBrvSensor
    {
        private const uint M = 2147483648; // modulo = 2^31
        private const uint B = 65539; // B - parameter = 2^16 + 3 
        private uint _a; //previous value

        public LinearCongruentialGenerator(uint seed = B)
        {
            _a = seed % M; // initialize first value, default initialization is B - parameter
        }

        public double Generate()
        {
            _a = _a * B % M; // Generate new value
            return (double)_a / M; //normalize to [0..1) 
        }
    }
}