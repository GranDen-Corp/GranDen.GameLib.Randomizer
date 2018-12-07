using MathNet.Numerics.Random;

namespace RandomLib
{
    public class Util
    {
        public static RandomSource DefaultRandomSource(bool threadSafe = false)
        {
            return new SystemRandomSource(RandomSeed.Guid(), threadSafe);
        }
    }
}