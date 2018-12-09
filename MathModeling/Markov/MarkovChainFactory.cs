using MathModeling.Brv;

namespace MathModeling.Markov
{
    public class MarkovChainFactory : IMarkovChainFactory
    {
        private readonly IBrvSensor _sensor;
        private readonly double[,] _stateProbability;
        private readonly int _initialState;

        public MarkovChainFactory(IBrvSensor sensor, double[,] stateProbability, int initialState = 0)
        {
            _sensor = sensor;
            _stateProbability = stateProbability;
            _initialState = initialState;
        }

        public IMarkovChain Create()
        {
            return new MarkovChain(_sensor, _stateProbability, _initialState);
        }
    }
}