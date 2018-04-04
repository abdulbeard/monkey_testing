//using System;
//using System.Collections.Generic;
//using System.Linq;
//using CuriousGeorge;
//using CuriousGeorge.mstest;
//using Xunit;

//namespace Tests
//{
//    public class ClassTests
//    {
//        [Fact]
//        public void GetDataTest()
//        {
//            var methodInfo = typeof(ClassTests).GetMethod(nameof(GuineaPigFunction));
//            var testCases = new CuriousGeorge.xunit.MonkeyTest(typeof(ClassTests), nameof(GuineaPigFunction), true)
//                .GetData(methodInfo).ToList();
//            Assert.Equal(20, testCases.Count());
//            var firstArgDistinct = testCases.Select(x => x.First()).Distinct().ToList();
//            var firstArgDistinctNotNull = firstArgDistinct.Where(x => x != null).ToList();
//            var secondArgDistinct = testCases.Select(x => x.ElementAt(1)).Distinct().ToList();
//            Assert.Equal(5, firstArgDistinct.Count);
//            Assert.Equal(4, secondArgDistinct.Count);
//            Assert.Single(firstArgDistinctNotNull.Where(x => ((PocoWrapper<TestClass>)x).Payload.Children == null));
//            Assert.Single(firstArgDistinctNotNull.Where(x =>
//                ((PocoWrapper<TestClass>)x).Payload.Children?.Count == 0));
//            Assert.Single(firstArgDistinctNotNull.Where(x => ((PocoWrapper<TestClass>)x).Payload.Children?.Count == 3));
//            Assert.Single(firstArgDistinctNotNull.Where(x => ((PocoWrapper<TestClass>)x).Payload.Children?.Count == 6));
//            Assert.Equal(2,
//                firstArgDistinctNotNull.Count(x =>
//                    ((PocoWrapper<TestClass>)x).Payload.Children?.Count(y => y != null) == 3));
//            Assert.Single(firstArgDistinctNotNull.Where(x =>
//                ((PocoWrapper<TestClass>)x).Payload.Children?.Count(y => y == null) == 3));
//        }

//        public void GuineaPigFunction(PocoWrapper<TestClass> a, int b)
//        {
//        }
//    }

//    public class TestClass
//    {
//        public List<string> Children { get; set; }
//        public int Count { get; set; }
//        public string Yolo { get; set; }
//        public Guid Id { get; set; }
//    }
//}
