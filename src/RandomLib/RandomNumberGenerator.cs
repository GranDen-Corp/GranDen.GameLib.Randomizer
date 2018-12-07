using System;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace RandomLib
{
    /// <summary>
    /// Helper class for easily generate random number
    /// </summary>
    public class RandomNumberGenerator
    {
        /// <summary>
        /// Generate a random Integer from [minValue , maxValue]
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static int CreateRandomInt(int minValue, int maxValue, RandomSource rng = null)
        {
            if (rng == null)
            {
                rng = Util.DefaultRandomSource(true);
            }

            return DiscreteUniform.Sample(rng, minValue, maxValue);
        }

        /// <summary>
        /// Generate a random Double from [minValue , maxValue]
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static double CreateRandomDouble(double minValue, double maxValue, RandomSource rng = null)
        {
            if (rng == null)
            {
                rng = Util.DefaultRandomSource(true);
            }

            return ContinuousUniform.Sample(rng, minValue, maxValue);
        }


    }
}