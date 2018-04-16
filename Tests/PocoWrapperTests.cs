using System;
using System.Collections.Generic;
using CuriousGeorge;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class PocoWrapperTests
    {
        [Fact]
        public void PocoWrapper_Serialize_Test()
        {
            var testClass = new TestClass()
            {
                Children = new List<string> {"a", "b", "c"},
                Count = 3,
                Id = Guid.Parse("787cd377-0ba2-4796-93a1-aa0fb96c06c0"),
                Yolo = "Nolo"
            };
            var wrapper = new PocoWrapper<TestClass>(testClass);
            var serializationInfo = new Xunitserializationinfo();
            wrapper.Serialize(serializationInfo);
            var retrievedValue = serializationInfo.GetValue<string>("serializedValue");
            Assert.Equal("{\"Children\":[\"a\",\"b\",\"c\"],\"Count\":3,\"Yolo\":\"Nolo\",\"Id\":\"787cd377-0ba2-4796-93a1-aa0fb96c06c0\"}", retrievedValue);
        }

        [Fact]
        public void PocoWrapper_Deserialize_Test()
        {
            var testClass = new TestClass()
            {
                Children = new List<string> { "a", "b", "c" },
                Count = 3,
                Id = Guid.NewGuid(),
                Yolo = "Nolo"
            };
            var wrapper = new PocoWrapper<TestClass>(testClass);
            var serializationInfo = new Xunitserializationinfo();
            wrapper.Serialize(serializationInfo);
            serializationInfo.AddValue("serializedValue", JsonConvert.SerializeObject(new TestClass() {Count = 465}));
            Assert.Equal(3, wrapper.Payload.Count);
            wrapper.Deserialize(serializationInfo);
            Assert.Equal(465, wrapper.Payload.Count);
        }
    }

    public class Xunitserializationinfo : IXunitSerializationInfo
    {
        private Dictionary<string, object> dict = new Dictionary<string, object>();
        public void AddValue(string key, object value, Type type = null)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else dict.Add(key, value);
        }

        public object GetValue(string key, Type type)
        {
            return dict[key];
        }

        public T GetValue<T>(string key)
        {
            return (T) Convert.ChangeType(GetValue(key, typeof(T)), typeof(T));
        }
    }
}
