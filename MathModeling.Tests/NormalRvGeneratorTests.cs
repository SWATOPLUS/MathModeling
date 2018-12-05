using MathModeling.Brv;
using MathModeling.Rv;
using Xunit;

namespace MathModeling.Tests
{
    public class NormalRvGeneratorTests
    {
        [Fact]
        public void NormalRvGenerator_RarefiedBoundAreCorrect()
        {
            var sensor = new LinearCongruentialGenerator();
            var generator = new NormalRvGenerator(sensor, 10, 100);

            var distributionFunction = generator.DistributionFunction;
            var lower = generator.LowerRarefiedBound;
            var upper = generator.UpperRarefiedBound;

            Assert.True(distributionFunction(lower) <= RvUtils.RarefiedBoundEps);
            Assert.True(distributionFunction(upper) >= 1 - RvUtils.RarefiedBoundEps);
        }
    }
}
