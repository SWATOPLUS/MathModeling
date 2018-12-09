namespace MathModeling.Markov
{
    public interface IMarkovChain
    {
        int State { get; }

        int NextState();
    }
}