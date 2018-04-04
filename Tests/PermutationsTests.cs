using System;
using CuriousGeorge;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class PermutationsTests
    {
        [Fact]
        public void AbdulsAlgorithm()
        {
            var abdulResult =
                Permutations.AbdulsAlgorithm(
                    new List<List<object>>()
                    {
                        new List<object>() {"1", "2"},
                        new List<object>() {"1", "2", "3"},
                        new List<object>() {"1", "2"}
                    });
            var results = abdulResult.Select(x => string.Join(',', x)).ToList();
            Assert.True(results.Distinct().Count() == results.Count);
            Assert.Equal(12, results.Count);
            Assert.Contains("1,1,1", results);
        }

        [Theory]
        [InlineData("1,1,1")]
        [InlineData("1,1,2")]
        [InlineData("1,2,1")]
        [InlineData("1,2,2")]
        [InlineData("1,3,1")]
        [InlineData("1,3,2")]
        [InlineData("2,1,1")]
        [InlineData("2,1,2")]
        [InlineData("2,2,1")]
        [InlineData("2,2,2")]
        [InlineData("2,3,1")]
        [InlineData("2,3,2")]
        public void AbdulsAlgorithm_AllCombinationsTest(string combinationAsString)
        {

            var abdulResult =
                Permutations.AbdulsAlgorithm(
                    new List<List<object>>()
                    {
                        new List<object>() {"1", "2"},
                        new List<object>() {"1", "2", "3"},
                        new List<object>() {"1", "2"}
                    });
            var results = abdulResult.Select(x => string.Join(',', x)).ToList();
            Assert.Contains(combinationAsString, results);
        }

        [Theory]
        [InlineData("1,2,3")]
        [InlineData("1,3,2")]
        [InlineData("3,2,1")]
        [InlineData("2,1,3")]
        [InlineData("2,3,1")]
        [InlineData("3,1,2")]
        public void HeapsAlgorithmTest(string permuatation)
        {
            var heapsResult = Permutations.HeapsAlgorithm(3, new[] { "1", "2", "3" }).Select(x => string.Join(",", x)).ToList();
            Assert.Equal(6, heapsResult.Count);
            Assert.Contains(permuatation, heapsResult);
        }

        [Fact]
        public void GetDataTest()
        {
            var methodInfo = typeof(PermutationsTests).GetMethod(nameof(GuineaPigFunction));
            var testCases = new CuriousGeorge.xunit.MonkeyTest(typeof(PermutationsTests), nameof(GuineaPigFunction), true)
                .GetData(methodInfo).ToList();
            Assert.Equal(20, testCases.Count);
            var firstArgDistinct = testCases.Select(x => x.First()).Distinct().ToList();
            var firstArgDistinctNotNull = firstArgDistinct.Where(x => x != null).ToList();
            var secondArgDistinct = testCases.Select(x => x.ElementAt(1)).Distinct().ToList();
            Assert.Equal(5, firstArgDistinct.Count);
            Assert.Equal(4, secondArgDistinct.Count);
            Assert.Single(firstArgDistinctNotNull.Where(x => ((PocoWrapper<Tests.TestClass>)x).Payload.Children == null));
            Assert.Single(firstArgDistinctNotNull.Where(x =>
                ((PocoWrapper<Tests.TestClass>)x).Payload.Children?.Count == 0));
            Assert.Single(firstArgDistinctNotNull.Where(x => ((PocoWrapper<Tests.TestClass>)x).Payload.Children?.Count == 3));
            Assert.Single(firstArgDistinctNotNull.Where(x => ((PocoWrapper<Tests.TestClass>)x).Payload.Children?.Count == 6));
            Assert.Equal(2,
                firstArgDistinctNotNull.Count(x =>
                    ((PocoWrapper<TestClass>)x).Payload.Children?.Count(y => y != null) == 3));
            Assert.Single(firstArgDistinctNotNull.Where(x =>
                ((PocoWrapper<TestClass>)x).Payload.Children?.Count(y => y == null) == 3));
        }

        public void GuineaPigFunction(PocoWrapper<Tests.TestClass> a, int b)
        {
        }
    }

    public class TestClass
    {
        public List<string> Children { get; set; }
        public int Count { get; set; }
        public string Yolo { get; set; }
        public Guid Id { get; set; }
    }
}
