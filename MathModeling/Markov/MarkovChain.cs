using MathModeling.Brv;
using MathModeling.Utils;
using System.Linq;

namespace MathModeling.Markov
{
    public class MarkovChain : IMarkovChain
    {
        private readonly IBrvSensor _sensor;
        private readonly double[,] _stateProbability;

        public MarkovChain(IBrvSensor sensor, double[,] stateProbability, int initialState = 0)
        {
            _sensor = sensor;
            _stateProbability = stateProbability;
            State = initialState;
        }

        public int State { get; private set; }

        public int NextState()
        {
            //todo: refactor this to improve performance
            var eta = _sensor.Generate();

            foreach (var (x, i) in _stateProbability.GetRow(State).Select((x, i) => (x, i)))
            {
                eta -= x;

                if (eta <= 0.0)
                {
                    State = i;
                    return State;
                }
            }

            State = _stateProbability.GetLength(1);

            return State;
        }
    }
}
