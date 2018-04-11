using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

namespace CuriousGeorge
{
    public static class MonkeyTestUtils
    {
        private static readonly string PocoWrapperGenericName =
            $"{typeof(PocoWrapper<object>).GetGenericTypeDefinition().Name}";

        private static readonly string PocoWrapperNamespace = typeof(PocoWrapper<object>).Namespace;
        public static IEnumerable<object[]> GetAllPossibleCombinations(List<Type> methodArgumentTypes, Fixture fixture)
        {
            var collectionOfCollectionOfVariations = new List<List<object>>();
            foreach (var type in methodArgumentTypes)
            {
                var valuesForThisType = new List<object>
                {
                    GetValueByValueType(type, ValueType.Null, fixture),
                    GetValueByValueType(type, ValueType.Default, fixture),
                    GetValueByValueType(type, ValueType.Max, fixture),
                    GetValueByValueType(type, ValueType.Min, fixture),
                };
                var regular = GetValueByValueType(type, ValueType.RandomObject, fixture); //regular
                var emptyLists = GetValueByValueType(type, ValueType.RandomObject, new Fixture() {RepeatCount = 0}); // lists are empty
                var nullLists = MakeListsNull(GetValueByValueType(type, ValueType.RandomObject, fixture)); // lists are null
                var listsWithNulls = MakeListsHaveNulls(GetValueByValueType(type, ValueType.RandomObject, fixture)); // lists have null entries
                valuesForThisType.AddRange(new[] {regular, emptyLists, nullLists, listsWithNulls});

                if (type.IsEnum)
                {
                    var enumValues = Enum.GetValues(type);
                    valuesForThisType.AddRange(Enumerable.Range(0, enumValues.Length - 1)
                        .Select(x => Convert.ChangeType(enumValues.GetValue(x), type)).ToList());
                }
                valuesForThisType = valuesForThisType.Distinct().ToList();
                collectionOfCollectionOfVariations.Add(valuesForThisType);
            }
            return Permutations.AbdulsAlgorithm(collectionOfCollectionOfVariations);
        }

        public static object MakeListsHaveNulls(object input)
        {
            var inputType = input?.GetType();
            var payloadProperty = inputType?.GetProperty(nameof(PocoWrapper<object>.Payload));
            var payloadPropertyType = payloadProperty?.PropertyType;
            var payloadValue = payloadProperty?.GetValue(input);
            var properties = inputType?.Namespace == PocoWrapperNamespace && inputType.Name == PocoWrapperGenericName
                ? payloadPropertyType.GetProperties()
                : inputType?.GetProperties();

            foreach (var property in properties ?? new PropertyInfo[0])
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType)
                {
                    var genericType = propertyType.GetGenericTypeDefinition();
                    if (genericType == typeof(List<>))
                    {
                        var enumerableAddMethodInfo = propertyType.GetMethods()
                            .Single(method => method.Name == nameof(List<object>.Add));
                        var value = property.GetValue(payloadValue);
                        enumerableAddMethodInfo.Invoke(value, new object[] { null });
                        enumerableAddMethodInfo.Invoke(value, new object[] { null });
                        enumerableAddMethodInfo.Invoke(value, new object[] { null });
                        property.SetValue(payloadValue, value);
                    }
                }
            }
            payloadProperty?.SetValue(input, payloadValue);
            return input;
        }

        public static object MakeListsNull(object input)
        {
            var inputType = input?.GetType();
            var payloadProperty = inputType?.GetProperty(nameof(PocoWrapper<object>.Payload));
            var payloadPropertyType = payloadProperty?.PropertyType;
            var payloadValue = payloadProperty?.GetValue(input);
            var properties = inputType?.Namespace == PocoWrapperNamespace && inputType.Name == PocoWrapperGenericName
                ? payloadPropertyType.GetProperties()
                : inputType?.GetProperties();
            foreach (var property in properties ?? new PropertyInfo[0])
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType)
                {
                    var genericTypeDef = propertyType.GetGenericTypeDefinition();
                    if (genericTypeDef == typeof(List<>))
                    {
                        property.SetValue(payloadValue, null);
                    }
                }
            }
            payloadProperty?.SetValue(input, payloadValue);
            return input;
        }

        public static IEnumerable<object[]> GetData(List<Type> methodArgumentTypes, bool allPossibleCombinations, Fixture fixture)
        {
            if (allPossibleCombinations)
            {
                return GetAllPossibleCombinations(methodArgumentTypes, fixture);
            }
            return new List<object[]>
            {
                GetNullValues(methodArgumentTypes, fixture),
                GetDefaultValues(methodArgumentTypes, fixture),
                GetMaxValues(methodArgumentTypes, fixture),
                GetMinValues(methodArgumentTypes, fixture),
                GetRandomObjectValues(methodArgumentTypes, fixture)
            };
        }

        public static object[] GetRandomObjectValues(IEnumerable<Type> types, Fixture fixture)
        {
            return types.Select(type => GetValueByValueType(type, ValueType.RandomObject, fixture)).ToArray();
        }

        public static object[] GetNullValues(IEnumerable<Type> types, Fixture fixture)
        {
            return types.Select(type => GetValueByValueType(type, ValueType.Null, fixture)).ToArray();
        }

        public static object[] GetDefaultValues(IEnumerable<Type> types, Fixture fixture)
        {
            return types.Select(type => GetValueByValueType(type, ValueType.Default, fixture)).ToArray();
        }

        public static object[] GetMaxValues(IEnumerable<Type> types, Fixture fixture)
        {
            return types.Select(type => GetValueByValueType(type, ValueType.Max, fixture)).ToArray();
        }

        public static object[] GetMinValues(IEnumerable<Type> types, Fixture fixture)
        {
            return types.Select(type => GetValueByValueType(type, ValueType.Min, fixture)).ToArray();
        }

        public static object GetValueByValueType(Type type, ValueType valueType, ISpecimenBuilder fixture)
        {
            if (DataVariationsByType.TypeIsCovered(type))
            {
                if (valueType != ValueType.RandomObject)
                {
                    return GetValueFromDataTypeVariation(DataVariationsByType.GetDataTypeVariations(type), valueType);
                }
            }
            else if (type.IsEnum)
            {
                var variations = new DataHolder()
                {
                    MinValue = Enum.Parse(type, Enum.GetValues(type).Cast<int>().Min().ToString()),
                    MaxValue = Enum.Parse(type, Enum.GetValues(type).Cast<int>().Max().ToString()),
                    DefaultValue = Enum.Parse(type, Enum.GetValues(type).Cast<int>().Min().ToString()),
                    NullValue = Enum.Parse(type, Enum.GetValues(type).Cast<int>().Min().ToString())
                };
                return GetValueFromDataTypeVariation(variations, valueType);
            }
            else if (valueType == ValueType.RandomObject && !type.IsInterface && !type.IsAbstract)
            {
                try
                {
                    var obj = new SpecimenContext(fixture).Resolve(type);
                    return obj;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public static object GetValueFromDataTypeVariation(DataHolder variations, ValueType valueType)
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
    }
}
