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
            Assert.Pass();
        }
    }
}