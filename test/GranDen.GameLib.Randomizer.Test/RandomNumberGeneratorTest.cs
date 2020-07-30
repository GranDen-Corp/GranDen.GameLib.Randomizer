using Xunit;
using Xunit.Abstractions;

namespace GranDen.GameLib.Randomizer.Test
{
    public class RandomIntTest
    {
        private readonly ITestOutputHelper _output;

        public RandomIntTest(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 10)]
        [InlineData(-10, -1)]
        public void TestRandomInt(int minValue, int maxValue)
        {
            var chosen = RandomNumberGenerator.CreateRandomInt(minValue, maxValue);
            _output.WriteLine("chosen= {0}", chosen);
            Assert.True(chosen >= minValue && chosen <= maxValue);
        }
    }

    public class RandomDoubleTest
    {
        private readonly ITestOutputHelper _output;

        public RandomDoubleTest(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }

        [Theory]
        [InlineData(0.0, 1.0)]
        [InlineData(1.0, 10.0)]
        [InlineData(-10.0, -1.0)]
        public void TestRandomFloat(float minValue, float maxValue)
        {
            var chosen = RandomNumberGenerator.CreateRandomFloat(minValue, maxValue);
            _output.WriteLine("chosen= {0}", chosen);
            Assert.True(chosen >= minValue && chosen <= maxValue);
        }

        [Theory]
        [InlineData(0.0, 1.0)]
        [InlineData(1.0, 10.0)]
        [InlineData(-10.0, -1.0)]
        public void TestRandomDouble(double minValue, double maxValue)
        {
            var chosen = RandomNumberGenerator.CreateRandomDouble(minValue, maxValue);
            _output.WriteLine("chosen= {0}", chosen);
            Assert.True(chosen >= minValue && chosen <= maxValue);
        }
    }
}