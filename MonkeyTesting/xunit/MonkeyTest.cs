using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using Xunit;

namespace CuriousGeorge.xunit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MonkeyTest: ClassDataAttribute
    {
        private readonly MethodInfo _methodInfo;
        private readonly bool _allPossibleCombinations;
        private readonly Fixture _fixture;
        public MonkeyTest(Type classType, bool allPossibleCombinations = false) : base(classType)
        {
            _fixture = new Fixture
            {
                RepeatCount = 3
            };
            _allPossibleCombinations = allPossibleCombinations;
        }
        public MonkeyTest(Type classType, string methodName, bool allPossibleCombinations = false): base(classType)
        {
            try
            {
                var methodInfo = classType.GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                _methodInfo = methodInfo;
                _allPossibleCombinations = allPossibleCombinations;
                _fixture = new Fixture
                {
                    RepeatCount = 3
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not find method '{methodName}' on type {classType.FullName}",
                    nameof(methodName), ex);
            }
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var methodArgumentTypes = testMethod.GetParameters().Select(x => x.ParameterType).ToList();
            return MonkeyTestUtils.GetData(methodArgumentTypes, _allPossibleCombinations, _fixture);
        }
    }
}
