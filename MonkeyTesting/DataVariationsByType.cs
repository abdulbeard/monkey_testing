using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CuriousGeorge
{
    public static class DataVariationsByType
    {
        public static bool TypeIsCovered(Type type)
        {
            return MonkeyValuesByType.ContainsKey(type);
        }

        public static DataHolder GetDataTypeVariations(Type type)
        {
            return MonkeyValuesByType.ContainsKey(type) ? MonkeyValuesByType[type] : null;
        }

        private static readonly Dictionary<Type, DataHolder> MonkeyValuesByType = new Dictionary<Type, DataHolder>()
        {
            {typeof(byte), new DataHolder
            {
                NullValue = 0,
                DefaultValue = default(byte),
                MaxValue = byte.MaxValue,
                MinValue = byte.MinValue
            }},
            {typeof(sbyte), new DataHolder
            {
                NullValue = 0,
                DefaultValue = 0,
                MaxValue = sbyte.MaxValue,
                MinValue = sbyte.MinValue
            }},
            {typeof(int), new DataHolder
            {
                NullValue = 0,
                DefaultValue = default(int),
                MaxValue = int.MaxValue,
                MinValue = int.MinValue
            }},
            {typeof(uint), new DataHolder
            {
                NullValue = 0,
                DefaultValue = default(uint),
                MaxValue = uint.MaxValue,
                MinValue = uint.MinValue
            }},
            {typeof(short), new DataHolder
            {
                NullValue = 0,
                DefaultValue = default(short),
                MaxValue = short.MaxValue,
                MinValue = short.MinValue
            }},
            {typeof(ushort), new DataHolder
            {
                NullValue = 0,
                DefaultValue = default(ushort),
                MaxValue = ushort.MaxValue,
                MinValue = ushort.MinValue
            }},
            {typeof(long), new DataHolder
            {
                NullValue = 0,
                DefaultValue = default(long),
                MaxValue = long.MaxValue,
                MinValue = long.MinValue
            }},
            {typeof(ulong), new DataHolder
            {
                NullValue = 0,
                DefaultValue = default(ulong),
                MaxValue = ulong.MaxValue,
                MinValue = ulong.MinValue
            }},
            {typeof(float), new DataHolder
            {
                NullValue = 0.0f,
                DefaultValue = default(float),
                MaxValue = float.MaxValue,
                MinValue = float.MinValue
            }},
            {typeof(double), new DataHolder
            {
                NullValue = 0.0d,
                DefaultValue = default(double),
                MaxValue = double.MaxValue,
                MinValue = double.MinValue
            }},
            {typeof(char), new DataHolder
            {
                NullValue = '\0',
                DefaultValue = default(char),
                MaxValue = char.MaxValue,
                MinValue = char.MinValue
            }},
            {typeof(bool), new DataHolder
            {
                NullValue = false,
                DefaultValue = default(bool),
                MaxValue = false,
                MinValue = false
            }},
            {typeof(string), new DataHolder
            {
                NullValue = null,
                DefaultValue = default(string),
                MaxValue = GetStringMax(),
                MinValue = string.Empty
            }},
            {typeof(decimal), new DataHolder
            {
                NullValue = 0.0M,
                DefaultValue = default(decimal),
                MaxValue = decimal.MaxValue,
                MinValue = decimal.MinValue
            }},
            {typeof(DateTime), new DataHolder
            {
                NullValue = DateTime.MinValue,
                DefaultValue = default(DateTime),
                MaxValue = DateTime.MaxValue,
                MinValue = DateTime.MinValue
            }},
            {typeof(object), new DataHolder
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


    }
}
