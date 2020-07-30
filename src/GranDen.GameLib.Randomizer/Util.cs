using MathNet.Numerics.Random;

namespace GranDen.GameLib.Randomizer
{
    /// <summary>
    /// Utility class
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Create a default Math.NET Random seed source.
        /// </summary>
        /// <param name="threadSafe">set to true to prevent generate the same random value on multi-thread environment.</param>
        /// <returns></returns>
        public static RandomSource DefaultRandomSource(bool threadSafe = false)
        {
            return new SystemRandomSource(RandomSeed.Guid(), threadSafe);
        }
    }
}
