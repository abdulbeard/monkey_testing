using CuriousGeorge;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class CounterTests
    {
        [Fact]
        public void SingleCounter_ResetTest()
        {
            var counter = new Counter(null, 3, 0);
            Assert.Equal(0, counter.GetValue());
            counter.Increment();
            Assert.Equal(1, counter.GetValue());
            counter.Increment();
            Assert.Equal(2, counter.GetValue());
            counter.Increment();
            Assert.Equal(0, counter.GetValue());
        }

        [Fact]
        public void SingleCounter_ResetTest_OnThresholdReached()
        {
            var list = new List<string>();
            var counter = new Counter(null, 3, 0);
            counter.ThresholdReached += (sender, eventArgs) => { list.Add("thresholdReached"); };
            Assert.Equal(0, counter.GetValue());
            counter.Increment();
            Assert.Equal(1, counter.GetValue());
            counter.Increment();
            Assert.Equal(2, counter.GetValue());
            counter.Increment();
            Assert.Equal(0, counter.GetValue());
            Assert.Single(list);
            Assert.Contains("thresholdReached", list);
        }

        [Fact]
        public void MultipleCounters()
        {
            var counter1 = new Counter(null, 10, 1);
            var counter2 = new Counter(counter1, 10, 1);
            var counter3 = new Counter(counter2, 10, 1);
            Assert.Equal(1, counter1.GetValue());
            Assert.Equal(1, counter2.GetValue());
            Assert.Equal(1, counter3.GetValue());
            for (var i = 0; i < 9; i++)
            {
                counter1.Increment();
            }
            Assert.Equal(1, counter1.GetValue());
            Assert.Equal(2, counter2.GetValue());
            Assert.Equal(1, counter3.GetValue());
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    counter1.Increment();
                }
            }
            Assert.Equal(1, counter1.GetValue());
            Assert.Equal(2, counter2.GetValue());
            Assert.Equal(2, counter3.GetValue());
        }
    }
}
