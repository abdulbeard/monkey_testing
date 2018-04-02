using CuriousGeorge.nunit;
using NUnit.Framework;

namespace CuriousGeorge.Tests.NUnit
{
    public class Tests
    {
        [Test]
        [TestCaseSource(typeof(MonkeyTestCaseSource), "GetData", new object[] {typeof(Tests), nameof(Test1), true})]
        public void Test1(int a, int b, int c)
        {
            var result = new CodeToTest().FunctionToTest1(a, b, c);
            Assert.True(result <= int.MaxValue);
            Assert.Pass();
        }
    }

    public class CodeToTest
    {
        public int FunctionToTest1(int quantity, int count, int multiplier)
        {
            return quantity * count * multiplier;
        }
    }
}