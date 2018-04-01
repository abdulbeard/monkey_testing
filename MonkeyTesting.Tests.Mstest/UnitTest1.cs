using System;
using System.Collections.Generic;
using CuriousGeorge.mstest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuriousGeorge.Tests.Mstest
{
    [TestClass]
    public class UnitTest1
    {
        [DataTestMethod]
        [MonkeyTest(typeof(UnitTest1), nameof(IAmASuperDuperTest), true)]
        public void IAmASuperDuperTest(TestingType tt, int a)
        {
        }
    }

    public class TestingType
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
        public List<string> Attributes { get; set; }
    }
}
