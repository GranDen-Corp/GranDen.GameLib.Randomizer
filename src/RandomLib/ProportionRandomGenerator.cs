using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public SortedDictionary<T, double> ProbabilityEntries { get; set; }

        /// <summary>
        /// Select one random choice
        /// </summary>
        /// <returns>The chosen one</returns>
        public T Draw()
        {
            var luckyOne = ProbabilityEntries.ElementAt(Categorical.Sample(_rng, ProbabilityEntries.Values.ToArray()));
            return luckyOne.Key;
        }

        /// <summary>
        /// Consecutively select multiple choices, results may be duplicated
        /// </summary>
        /// <param name="times">Draw counts</param>
        /// <returns>Chosen entries</returns>
        public IEnumerable<T> DuplicatedConsecutiveDraws(int times)
        {
            if (times <= 0)
            {
                throw new ArgumentException("Draw times must be positive integer!");
            }

            //if we only have one choice...
            if (ProbabilityEntries.Count == 1)
            {
                return Enumerable.Repeat(ProbabilityEntries.First().Key, times);
            }

            var categoricalDist = CreateCategoricalDistribution();

            var resultIndexes = new int[times];
            categoricalDist.Samples(resultIndexes);

            var results = new List<T>(times);
            results.AddRange(resultIndexes.Select(resultIndex => ProbabilityEntries.ElementAt(resultIndex).Key));

            return results;
        }

        private Categorical CreateCategoricalDistribution()
        {
            return new Categorical(ProbabilityEntries.Values.ToArray(), _rng);
        }

        /// <summary>
        /// Consecutively select multiple choices and may go on until all items were chosen, results will not duplicated.
        /// </summary>
        /// <param name="times">Draw counts, must be less or equal to original probability entries</param>
        /// <returns>Chosen entries</returns>
        public IEnumerable<T> NonDuplicatedConsecutiveDraws(int times)
        {
            if (times <= 0)
            {
                throw new ArgumentException("Draw times must be positive integer!");
            }

            if (times > ProbabilityEntries.Count)
            {
                throw new ArgumentException("Exclusive draw times must be less than or equal to original probability entries");
            }

            //if we only have one choice...
            if (ProbabilityEntries.Count == 1)
            {
                return Enumerable.Repeat(ProbabilityEntries.First().Key, times);
            }

            var copiedProbabilityEntries = new Dictionary<T, double>(ProbabilityEntries);

            var results = new List<T>(times);
            while (times > 0)
            {
                var luckyOne =
                    copiedProbabilityEntries.ElementAt(
                        Categorical.Sample(_rng, copiedProbabilityEntries.Values.ToArray())).Key;

                results.Add(luckyOne);
                copiedProbabilityEntries.Remove(luckyOne);
                times--;
            }

            return results;
        }
    }
}