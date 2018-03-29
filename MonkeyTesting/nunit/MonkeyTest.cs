using System;
using System.Collections.Generic;
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
                return MonkeyTestUtils.GetData(methodInfo, allPossibleCombinations, fixture);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not find method '{methodName}' on type {classType.FullName}",
                    nameof(methodName), ex);
            }
        }
    }
}
