using Newtonsoft.Json;
using Xunit.Abstractions;

namespace CuriousGeorge
{
    public class PocoWrapper<T> : IXunitSerializable
    {
        public T Payload { get; set; }

        public PocoWrapper(){}

        public PocoWrapper(T payloadToSerialize)
        {
            Payload = payloadToSerialize;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            Payload = JsonConvert.DeserializeObject<T>(info.GetValue<string>("serializedValue"));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            var json = JsonConvert.SerializeObject(Payload);
            info.AddValue("serializedValue", json);
        }
    }
}
