using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace RandomLib
{
    /// <summary>
    /// Random select choice based on proportional probability
    /// </summary>
    /// <typeparam name="T">chosen choice object type</typeparam>
    public class ProportionRandomGenerator<T>
    {
        private readonly RandomSource _rng;

        /// <summary>
        /// Create the RNG object
        /// </summary>
        /// <param name="randomSource">optional, assigned it if you need controllable results like running unit test.</param>
        public ProportionRandomGenerator(RandomSource randomSource = null)
        {
            _rng = randomSource ?? Util.DefaultRandomSource(true);
        }

        /// <summary>
        /// Probability distribution data, probability values don't have to sum to a fixed total number.
        /// </summary>
        public SortedDictionary<T, double> ProbabilityEntries { get; set; }

        /// <summary>
        /// Select one random choice
        /// </summary>
        /// <returns></returns>
        public T Draw()
        {
            var luckyOne = ProbabilityEntries.ElementAt(Categorical.Sample(_rng, ProbabilityEntries.Values.ToArray()));
            return luckyOne.Key;
        }

        /// <summary>
        /// Consecutively select multiple choices, results may be duplicated
        /// </summary>
        /// <param name="times">Choose counts</param>
        /// <returns></returns>
        public IEnumerable<T> DuplicatedConsecutiveDraws(int times)
        {
            if (times <= 0)
            {
                throw new ArgumentException("draw times must be positive!");
            }

            var categoricalDist = new Categorical(ProbabilityEntries.Values.ToArray(), _rng);

            var resultIndexes = new int[times];
            categoricalDist.Samples(resultIndexes);

            var results = new List<T>(times);
            results.AddRange(resultIndexes.Select(resultIndex => ProbabilityEntries.ElementAt(resultIndex).Key));

            return results;
        }

        /// <summary>
        /// Consecutively select multiple choices and may go on until all items were chosen, results will not duplicated.
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public IEnumerable<T> NonDuplicatedConsecutiveDraws(int times)
        {
            if (times <= 0)
            {
                throw new ArgumentException("draw times must be positive!");
            }

            if (times > ProbabilityEntries.Count)
            {
                throw new ArgumentException("exclusive draw times must be less than origin probability entries");
            }

            var probabilityEntities = new SortedDictionary<T, double>();
            foreach (var kv in ProbabilityEntries)
            {
                probabilityEntities.Add(kv.Key, kv.Value);
            }

            var results = new List<T>(times);
            while (times > 0)
            {
                var luckyOne =
                    probabilityEntities.ElementAt(
                        Categorical.Sample(_rng, probabilityEntities.Values.ToArray())).Key;
                
                results.Add(luckyOne);
                probabilityEntities.Remove(luckyOne);
                times--;
            }

            return results;
        }
    }
}