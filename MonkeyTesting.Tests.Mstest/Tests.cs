using System;
using System.Collections.Generic;
using CuriousGeorge.mstest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuriousGeorge.Tests.Mstest
{
    [TestClass]
    public class Tests
    {
        [DataTestMethod]
        [MonkeyTest(typeof(Tests), nameof(IamASuperDuperTest), true)]
        public void IamASuperDuperTest(TestClass tt, int a)
        {
            var result = new CodeToTest().IamASuperDuperFunction(tt, a);
            Assert.IsTrue(result <= int.MaxValue);
        }
    }

    public class CodeToTest
    {
        public int IamASuperDuperFunction(TestClass tt, int multiplier)
        {
            return tt?.Count * multiplier ?? 0;
        }
    }

    public class TestClass
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
        public List<string> Attributes { get; set; }
    }
}
