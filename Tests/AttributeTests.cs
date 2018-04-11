using System;
using System.Linq;
using CuriousGeorge.mstest;
using CuriousGeorge.nunit;
using CuriousGeorge.xunit;
using Xunit;

namespace Tests
{
    public class AttributeTests
    {
        [Fact]
        public void Mstest_AttributeTest()
        {
            var methodInfo = typeof(AttributeTests).GetMethod(nameof(GuineaPigMethod));
            var monkeyTestAttribute = new MonkeyTestAttribute(true);
            var datasets = monkeyTestAttribute.GetData(methodInfo);
            Assert.Equal(40, datasets.Count());
            var name = monkeyTestAttribute.GetDisplayName(methodInfo, new object[] {"a", "b", "c"});
            Assert.Equal($"{nameof(GuineaPigMethod)}(a,b,c)", name);
        }

        [Fact]
        public void xunit_AttributeTest()
        {
            var methodInfo = typeof(AttributeTests).GetMethod(nameof(GuineaPigMethod));
            var monkeyTestAttribute = new MonkeyTest(typeof(AttributeTests), nameof(GuineaPigMethod), true);
            var datasets = monkeyTestAttribute.GetData(methodInfo);
            Assert.Equal(40, datasets.Count());

            monkeyTestAttribute = new MonkeyTest(typeof(AttributeTests), true);
            datasets = monkeyTestAttribute.GetData(methodInfo);
            Assert.Equal(40, datasets.Count());
        }

        [Fact]
        public void nunit_AttributeTest()
        {
            var datasets = MonkeyTestCaseSource.GetData(typeof(AttributeTests), nameof(GuineaPigMethod), true);
            Assert.Equal(40, datasets.Count());
        }

        [Fact]
        public void nunit_AttributeTest_NoSuchMethod()
        {
            Assert.Throws<ArgumentException>(() =>
                MonkeyTestCaseSource.GetData(typeof(AttributeTests), "IAmANonExistentMethod", true));
        }

        public void GuineaPigMethod(int a, string b, long c) { }
    }
}
