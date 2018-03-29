using System;

namespace CuriousGeorge
{
    public class Counter
    {
        private int _value;
        private readonly int _threshold;
        private readonly int _initialValue;

        public Counter(Counter child, int threshold, int initialValue)
        {
            _threshold = threshold;
            _value = initialValue;
            _initialValue = initialValue;
            if (child != null)
            {
                child.ThresholdReached += ChildCounterEventReceiver;
            }
        }

        public void Increment()
        {
            _value++;
            if (_value < _threshold) return;
            ResetCounter();
            OnThresholdReached(new EventArgs());
        }

        public int GetValue()
        {
            return _value;
        }

        public event EventHandler ThresholdReached;

        protected virtual void OnThresholdReached(EventArgs args)
        {
            var handler = ThresholdReached;
            handler?.Invoke(this, args);
        }

        private void ChildCounterEventReceiver(object sender, EventArgs args)
        {
            Increment();
        }

        private void ResetCounter()
        {
            _value = _initialValue;
        }
    }
}
