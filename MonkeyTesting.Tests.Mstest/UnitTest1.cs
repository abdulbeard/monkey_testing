using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuriousGeorge.Tests.Mstest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IAmASuperDuperTest()
        {
        }

        [DataTestMethod]
        [mstest.DynamicData(typeof(UnitTest1), nameof(AnotherIAmASuperDuperTest), true)]
        public void AnotherIAmASuperDuperTest(int a, int b, int c)
        {
            Console.WriteLine($"{a}-{b}-{c}");
        }
    }
}
