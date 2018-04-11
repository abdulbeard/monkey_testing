using System.Collections.Generic;

namespace Tests.TestingTypes
{
    /// <summary>
    /// Fake poco wrapper for test
    /// </summary>
    public class PocoWrapper<T>
    {
        public T Stuff { get; set; }
    }

    public class EmptyClass { }

    public class GenericClass
    {
        public IEnumerable<string> Payload { get; set; }
    }

    public class TestClassWithChildren
    {
        public List<TestClassWithChildren> Children { get; set; }
    }

    public enum Yolo
    {
        Yo,
        Lo,
        So,
        Low
    }

}
