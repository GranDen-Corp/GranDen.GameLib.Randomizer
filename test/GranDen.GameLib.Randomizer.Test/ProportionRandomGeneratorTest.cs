using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace GranDen.GameLib.Randomizer.Test
{
    public class ProportionRandomGeneratorTest
    {
        private readonly ITestOutputHelper _output;

        public ProportionRandomGeneratorTest(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }

        [Fact]
        public void TestDrawCustomObject()
        {
            //Arrange
            var prize1 = new Prize("{NO.1}");
            var prize2 = new Prize("{NO.2}");
            var prize3 = new Prize("{Super Big Prize!!!}");
            var prizeDict = new SortedDictionary<Prize, double>
            {
                [prize1] = 10,
                [prize2] = 50,
                [prize3] = 0.1,
            };
            var rng = new ProportionRandomGenerator<Prize> { ProbabilityEntries = prizeDict };

            //Act
            var luckyOne = rng.Draw();
            _output.WriteLine($"luckyOne={luckyOne}");

            //Assert
            Assert.True(luckyOne == prize1 || luckyOne == prize2 || luckyOne == prize3);
        }

        [Fact]
        public void TestDuplicatedConsecutiveDraw()
        {
            //Arrange
            var chosePool = new List<string>();
            for (var i = 1; i <= 10; i++)
            {
                chosePool.Add($"item{i:D2}");
            }
            var parameters = new SortedDictionary<string, double>();
            foreach (var item in chosePool)
            {
                parameters.Add(item, 1);
            }
            var rng = new ProportionRandomGenerator<string> { ProbabilityEntries = parameters };

            //Act
            var luckyPartialResults = rng.DuplicatedConsecutiveDraws(3).ToArray();
            _output.WriteLine($"lucky partial = [{luckyPartialResults.Aggregate((s1, s2) => $"{s1}, {s2}")}]");
            var luckyAllResults = rng.DuplicatedConsecutiveDraws(chosePool.Count).ToArray();
            _output.WriteLine($"lucky all = [{luckyAllResults.Aggregate((s1, s2) => $"{s1}, {s2}")}]");

            //Assert
            Assert.Equal(3, luckyPartialResults.Length);
            foreach (var luckyResult in luckyPartialResults)
            {
                Assert.Contains(luckyResult, chosePool);
            }

            Assert.Equal(chosePool.Count, luckyAllResults.Length);
            foreach (var luckyResult in luckyAllResults)
            {
                Assert.Contains(luckyResult, chosePool);
            }
        }

        [Fact]
        public void TestDuplicatedConsecutiveDrawOnSingleChoice()
        {
            //Arrange
            var parameter = new SortedDictionary<string, double>();
            parameter.Add("itemOnlyOne", 99.9999);
            var rng = new ProportionRandomGenerator<string> { ProbabilityEntries = parameter };

            //Act
            var luckyDrawResults = rng.DuplicatedConsecutiveDraws(10).ToArray();

            //Assert
            foreach (var lucky in luckyDrawResults)
            {
                Assert.Equal("itemOnlyOne", lucky);
            }
        }

        [Fact]
        public void TestNonDuplicatedConsecutiveDraw()
        {
            //Arrange
            var chosePool = new List<string>();
            for (var i = 1; i <= 10; i++)
            {
                chosePool.Add($"item{i:D2}");
            }
            var parameters = new SortedDictionary<string, double>();
            foreach (var item in chosePool)
            {
                parameters.Add(item, 1);
            }
            var rng = new ProportionRandomGenerator<string> { ProbabilityEntries = parameters };

            //Act
            var luckyPartialResults = rng.NonDuplicatedConsecutiveDraws(4).ToArray();
            _output.WriteLine($"lucky partial = [{luckyPartialResults.Aggregate((s1, s2) => $"{s1}, {s2}")}]");
            var luckAllResult = rng.NonDuplicatedConsecutiveDraws(chosePool.Count).ToArray();
            _output.WriteLine($"lucky all = [{luckAllResult.Aggregate((s1, s2) => $"{s1}, {s2}")}]");

            //Assert
            Assert.Equal(4, luckyPartialResults.Length);
            var usedPartialResults = new List<string>();
            foreach (var luckyResult in luckyPartialResults)
            {
                Assert.Contains(luckyResult, chosePool);
                Assert.DoesNotContain(luckyResult, usedPartialResults);
                usedPartialResults.Add(luckyResult);
            }

            Assert.Equal(chosePool.Count, luckAllResult.Length);
            var usedAllResults = new List<string>();
            foreach (var luckyResult in luckAllResult)
            {
                Assert.Contains(luckyResult, chosePool);
                Assert.DoesNotContain(luckyResult, usedAllResults);
                usedAllResults.Add(luckyResult);
            }
        }

        [Fact]
        public void TestNonDuplicatedConsecutiveDrawOnSingleChoice()
        {
            //Arrange
            var parameter = new SortedDictionary<string, double>();
            parameter.Add("itemOnlyOne", 99.9999);
            var rng = new ProportionRandomGenerator<string> { ProbabilityEntries = parameter };

            //Act
            var luckyPartialResults = rng.NonDuplicatedConsecutiveDraws(1).ToArray();

            //Assert
            Assert.Single(luckyPartialResults);
            Assert.Equal("itemOnlyOne", luckyPartialResults.First());
        }
    }

    #region Test Object Class

    internal class Prize : IComparable
    {
        public string Msg { get; set; }

        public Prize(string msg)
        {
            Msg = msg;
        }

        public int CompareTo(object obj)
        {
            if (obj is Prize input)
            {
                return string.Compare(Msg, input.Msg, StringComparison.Ordinal);
            }
            throw new Exception("object cannot convert to Prize!");
        }

        public override string ToString()
        {
            return Msg;
        }
    }

    #endregion     
}