using System;
using CuriousGeorge.xunit;
using Xunit;

namespace CuriousGeorge.Tests.Xunit
{
    public class Tests
    {
        [Theory]
        [MonkeyTest(typeof(Tests))]
        public void Test1(int a, int b, int c)
        {
            var result = new CodeToTest().FunctionToTest1(a, b, c);
            Assert.InRange(result, int.MinValue, int.MaxValue);
        }

        [Theory]
        [MonkeyTest(typeof(Tests), nameof(Test2))]
        public void Test2(int a, float b, decimal c)
        {
            var result = new CodeToTest().FunctionToTest2(a, b, c);
            Assert.InRange(result, int.MinValue, int.MaxValue);
        }

        [Theory]
        [MonkeyTest(typeof(Tests), nameof(Test3))]
        public void Test3(Guid a, float b, decimal c)
        {
            var result = new CodeToTest().FunctionToTest3(a, b, c);
            Assert.InRange(result, int.MinValue, int.MaxValue);
        }

        [Theory]
        [MonkeyTest(typeof(Tests), nameof(Test4))]
        public void Test4(Guid a, float b, DegreesOfCertainty c)
        {
            var result = new CodeToTest().FunctionToTest4(a, b, c);
            Assert.InRange(result, int.MinValue, int.MaxValue);
        }

        [Theory]
        [MonkeyTest(typeof(Tests), nameof(Test5), true)]
        public void Test5(DegreesOfCertainty c)
        {
            var result = new CodeToTest().FunctionToTest5(c);
            Assert.InRange(result, int.MinValue, int.MaxValue);
        }

        [Theory]
        [MonkeyTest(typeof(Tests), nameof(Test6), true)]
        public void Test6(PocoWrapper<TestClass> a, int b)
        {
            var payload = a?.Payload;
            var result = new CodeToTest().FunctionToTest6(payload, b);
            Assert.InRange(result, int.MinValue, int.MaxValue);
        }
    }

    public class CodeToTest
    {
        public int FunctionToTest6(TestClass metrics, int multiplier)
        {
            return metrics?.count * multiplier ?? 0;
        }

        public int FunctionToTest5(DegreesOfCertainty doc)
        {
            return (int) doc;
        }

        public int FunctionToTest4(Guid id, float price, DegreesOfCertainty howSureAreYou)
        {
            return (int) price * (int) howSureAreYou;
        }

        public int FunctionToTest3(Guid id, float price, decimal multiplier)
        {
            price = 1.0f;
            multiplier = 1.0M;
            return (int) ((int) price * multiplier);
        }

        public int FunctionToTest2(int quantity, float unitPrice, decimal extendedUnitPrice)
        {
            return (int) ((int) quantity * unitPrice * (double) extendedUnitPrice);
        }

        public int FunctionToTest1(int a, int b, int c)
        {
            return a * b * c;
        }
    }

    public class TestClass
    {
        public int count { get; set; }
        public string Yolo { get; set; }
        public Guid Id { get; set; }
    }

    public enum DegreesOfCertainty
    {
        Really,
        NoSeriously,
        AreYoForReal
    }
}
