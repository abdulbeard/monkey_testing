using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace MonkeyTesting
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MonkeyTest: ClassDataAttribute
    {
        private readonly MethodInfo _methodInfo;
        public MonkeyTest(Type classType, string methodName, bool allPossibleCombinations = false): base(classType)
        {
            try
            {
                var methodInfo = classType.GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                _methodInfo = methodInfo;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not find method '{methodName}' on type {classType.FullName}",
                    nameof(methodName), ex);
            }
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var result = new List<object[]>();
            var methodArgumentTypes = _methodInfo.GetParameters().Select(x => x.ParameterType).ToList();
            result.Add(GetNullValues(methodArgumentTypes));
            result.Add(GetDefaultValues(methodArgumentTypes));
            result.Add(GetMaxValues(methodArgumentTypes));
            result.Add(GetMinValues(methodArgumentTypes));
            return result;
        }

        private object[] GetNullValues(IEnumerable<Type> types)
        {
            return types.Select(type => GetValueByValueType(type, ValueType.Null)).ToArray();
        }

        private object[] GetDefaultValues(IEnumerable<Type> types)
        {
            return types.Select(type => GetValueByValueType(type, ValueType.Default)).ToArray();
        }

        private object[] GetMaxValues(IEnumerable<Type> types)
        {
            return types.Select(type => GetValueByValueType(type, ValueType.Max)).ToArray();
        }

        private object[] GetMinValues(IEnumerable<Type> types)
        {
            return types.Select(type => GetValueByValueType(type, ValueType.Min)).ToArray();
        }

        private object GetValueByValueType(Type type, ValueType valueType)
        {
            if (_monkeyValuesByType.ContainsKey(type))
            {
                if (valueType != ValueType.RandomObject)
                {
                    return GetValueFromDataTypeVariation(_monkeyValuesByType[type], valueType);
                }
            }
            else if (type.IsEnum)
            {
                var variations = new DataTypeVariations()
                {
                    MinValue = Enum.Parse(type, Enum.GetValues(type).Cast<int>().Min().ToString()),
                    MaxValue = Enum.Parse(type, Enum.GetValues(type).Cast<int>().Max().ToString()),
                    DefaultValue = Enum.Parse(type, Enum.GetValues(type).Cast<int>().Min().ToString()),
                    NullValue = Enum.Parse(type, Enum.GetValues(type).Cast<int>().Min().ToString())
                };
                return GetValueFromDataTypeVariation(variations, valueType);
            }
            return null;
        }

        private object GetValueFromDataTypeVariation(DataTypeVariations variations, ValueType valueType)
        {
            switch (valueType)
            {
                case ValueType.Default:
                {
                    return variations.DefaultValue;
                }
                case ValueType.Max:
                {
                    return variations.MaxValue;
                }
                case ValueType.Min:
                {
                    return variations.MinValue;
                }
                case ValueType.Null:
                {
                    return variations.NullValue;
                }
                default: return null;
            }
        }

        private readonly Dictionary<Type, DataTypeVariations> _monkeyValuesByType = new Dictionary<Type, DataTypeVariations>()
        {
            {typeof(byte), new DataTypeVariations
            {
                NullValue = 0,
                DefaultValue = default(byte),
                MaxValue = byte.MaxValue,
                MinValue = byte.MinValue
            }},
            {typeof(sbyte), new DataTypeVariations
            {
                NullValue = 0,
                DefaultValue = 0,
                MaxValue = sbyte.MaxValue,
                MinValue = sbyte.MinValue
            }},
            {typeof(int), new DataTypeVariations
            {
                NullValue = 0,
                DefaultValue = default(int),
                MaxValue = int.MaxValue,
                MinValue = int.MinValue
            }},
            {typeof(uint), new DataTypeVariations
            {
                NullValue = 0,
                DefaultValue = default(uint),
                MaxValue = uint.MaxValue,
                MinValue = uint.MinValue
            }},
            {typeof(short), new DataTypeVariations
            {
                NullValue = 0,
                DefaultValue = default(short),
                MaxValue = short.MaxValue,
                MinValue = short.MinValue
            }},
            {typeof(ushort), new DataTypeVariations
            {
                NullValue = 0,
                DefaultValue = default(ushort),
                MaxValue = ushort.MaxValue,
                MinValue = ushort.MinValue
            }},
            {typeof(long), new DataTypeVariations
            {
                NullValue = 0,
                DefaultValue = default(long),
                MaxValue = long.MaxValue,
                MinValue = long.MinValue
            }},
            {typeof(ulong), new DataTypeVariations
            {
                NullValue = 0,
                DefaultValue = default(ulong),
                MaxValue = ulong.MaxValue,
                MinValue = ulong.MinValue
            }},
            {typeof(float), new DataTypeVariations
            {
                NullValue = 0.0f,
                DefaultValue = default(float),
                MaxValue = float.MaxValue,
                MinValue = float.MinValue
            }},
            {typeof(double), new DataTypeVariations
            {
                NullValue = 0.0d,
                DefaultValue = default(double),
                MaxValue = double.MaxValue,
                MinValue = double.MinValue
            }},
            {typeof(char), new DataTypeVariations
            {
                NullValue = '\0',
                DefaultValue = default(char),
                MaxValue = char.MaxValue,
                MinValue = char.MinValue
            }},
            {typeof(bool), new DataTypeVariations
            {
                NullValue = false,
                DefaultValue = default(bool),
                MaxValue = false,
                MinValue = false
            }},
            {typeof(string), new DataTypeVariations
            {
                NullValue = null,
                DefaultValue = default(string),
                MaxValue = GetStringMax(),
                MinValue = string.Empty
            }},
            {typeof(decimal), new DataTypeVariations
            {
                NullValue = 0.0M,
                DefaultValue = default(decimal),
                MaxValue = decimal.MaxValue,
                MinValue = decimal.MinValue
            }},
            {typeof(DateTime), new DataTypeVariations
            {
                NullValue = DateTime.MinValue,
                DefaultValue = default(DateTime),
                MaxValue = DateTime.MaxValue,
                MinValue = DateTime.MinValue
            }},
            {typeof(object), new DataTypeVariations
            {
                NullValue = null,
                DefaultValue = default(object),
                MaxValue = null,
                MinValue = null
            }}
        };

        private static string GetStringMax()
        {
            const string seed = "Thequickbrownfoxjumpedoverthelazydog;";
            var stringBuilder = new StringBuilder();
            seed.ToCharArray().Select(x => stringBuilder.Append(seed));
            return stringBuilder.ToString();
        }

        private class DataTypeVariations
        {
            public object NullValue { get; set; }
            public object MinValue { get; set; }
            public object MaxValue { get; set; }
            public object DefaultValue { get; set; }
        }

        private enum ValueType
        {
            Null,
            Min,
            Max,
            Default,
            RandomObject
        }

        //private enum ValueTypes
        //{
        //    Bool,
        //    Byte,
        //    Char,
        //    Decimal,
        //    Double,
        //    Enum,
        //    Float,
        //    Int,
        //    Long,
        //    Sbyte,
        //    Short,
        //    Struct,
        //    Uint,
        //    Ulong,
        //    Ushort
        //}
    }
}
