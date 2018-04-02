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
    }
}
