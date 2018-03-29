using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CuriousGeorge.Tests.Xunit
{
    public class PermuationsTests
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
        }
    }
}
