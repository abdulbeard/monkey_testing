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
                    GetValueByValueType(type, ValueType.RandomObject, fixture),
                    GetValueByValueType(type, ValueType.RandomObject, fixture),
                    GetValueByValueType(type, ValueType.RandomObject, fixture)
                };
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

        public static IEnumerable<object[]> GetData(MethodInfo methodInfo, bool allPossibleCombinations, Fixture fixture)
        {
            var methodArgumentTypes = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();
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
                var variations = new DataTypeVariations()
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
                    return new SpecimenContext(fixture).Resolve(type);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public static object GetValueFromDataTypeVariation(DataTypeVariations variations, ValueType valueType)
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
