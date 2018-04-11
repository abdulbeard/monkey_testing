using CuriousGeorge;
using Xunit;

namespace Tests
{
    public class DataVariationsByTypeTests
    {
        [Fact]
        public void GetDataTypeVariationsTests()
        {
            var result = DataVariationsByType.GetDataTypeVariations(typeof(byte));
            Assert.NotNull(result);

            result = DataVariationsByType.GetDataTypeVariations(typeof(TestClass));
            Assert.Null(result);
        }
    }
}
