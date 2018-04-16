using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CuriousGeorge;
using Tests.TestingTypes;
using Xunit;
using ValueType = CuriousGeorge.ValueType;

namespace Tests
{
    public class MonkeyTestUtilsTests
    {
        [Fact]
        public void IsEnumTest()
        {
            var results = MonkeyTestUtils.GetAllPossibleCombinations(new List<Type> {typeof(Yolo), typeof(int)}, new Fixture()).ToList();
            Assert.Equal(20, results.Count);
            Assert.Equal(20, results.Distinct().Count());
        }

        [Fact]
        public void GetDataTest()
        {
            var data = MonkeyTestUtils.GetData(new List<Type> {typeof(Yolo), typeof(int)}, false, new Fixture()).ToList();
            Assert.Equal(5, data.Count);
            Assert.Equal(Yolo.Yo, (Yolo) data.ElementAt(0)[0]);
            Assert.Equal(0, (int) data.ElementAt(0)[1]);
            Assert.Equal(Yolo.Yo, (Yolo)data.ElementAt(1)[0]);
            Assert.Equal(0, (int)data.ElementAt(1)[1]);
            Assert.Equal(Yolo.Low, (Yolo)data.ElementAt(2)[0]);
            Assert.Equal(int.MaxValue, (int)data.ElementAt(2)[1]);
            Assert.Equal(Yolo.Yo, (Yolo)data.ElementAt(3)[0]);
            Assert.Equal(int.MinValue, (int)data.ElementAt(3)[1]);
            Assert.Null(data.ElementAt(4)[0]);
            Assert.Null(data.ElementAt(4)[1]);
        }

        [Fact]
        public void GetValueByValueTypeTest_Exception()
        {
            var result = MonkeyTestUtils.GetValueByValueType(typeof(TestClassWithChildren), ValueType.RandomObject,
                new Fixture());
            Assert.Null(result);
        }

        [Fact]
        public void MakeListsHaveNullsTest_NullInput()
        {
            Assert.Null(MonkeyTestUtils.MakeListsHaveNulls(null));
            Assert.Null(((GenericClass)MonkeyTestUtils.MakeListsHaveNulls(new GenericClass())).Payload);
            Assert.NotNull((EmptyClass)MonkeyTestUtils.MakeListsHaveNulls(new EmptyClass()));
            Assert.NotNull(
                ((GenericClass)MonkeyTestUtils.MakeListsHaveNulls(new GenericClass()
                {
                    Payload = new List<string> { "a", "b", "c" }
                })).Payload);
            Assert.NotNull(MonkeyTestUtils.MakeListsHaveNulls(new CuriousGeorge.PocoWrapper<Counter>()));
            Assert.NotNull(MonkeyTestUtils.MakeListsHaveNulls(new CuriousGeorge.PocoWrapper<object>()));
            Assert.NotNull(MonkeyTestUtils.MakeListsHaveNulls(new CuriousGeorge.PocoWrapper<EmptyClass>()));
        }

        [Fact]
        public void MakeListsNullTest_NullInput()
        {
            Assert.Null(MonkeyTestUtils.MakeListsNull(null));
            Assert.Null(((GenericClass)MonkeyTestUtils.MakeListsNull(new GenericClass())).Payload);
            Assert.NotNull((EmptyClass) MonkeyTestUtils.MakeListsNull(new EmptyClass()));
            Assert.NotNull(
                ((GenericClass)MonkeyTestUtils.MakeListsNull(new GenericClass()
                {
                    Payload = new List<string> { "a", "b", "c" }
                })).Payload);
            Assert.NotNull(MonkeyTestUtils.MakeListsNull(new CuriousGeorge.PocoWrapper<Counter>()));
            Assert.NotNull(MonkeyTestUtils.MakeListsNull(new CuriousGeorge.PocoWrapper<object>()));
            Assert.NotNull(MonkeyTestUtils.MakeListsNull(new CuriousGeorge.PocoWrapper<EmptyClass>()));
        }
    }
}
