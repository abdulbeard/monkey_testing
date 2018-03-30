using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;

namespace CuriousGeorge.nunit
{
    public class MonkeyTestCaseSource
    {
        public static IEnumerable<object[]> GetData(Type classType, string methodName, bool allPossibleCombinations)
        {
            try
            {
                var methodInfo = classType.GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                var fixture = new Fixture {RepeatCount = 3};
                var methodArgumentTypes = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();
                return MonkeyTestUtils.GetData(methodArgumentTypes, allPossibleCombinations, fixture);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not find method '{methodName}' on type {classType.FullName}",
                    nameof(methodName), ex);
            }
        }
    }
}
