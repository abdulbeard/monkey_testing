using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuriousGeorge.mstest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MonkeyTestAttribute: Attribute, ITestDataSource
    {
        private readonly bool _allPossibleCombinations;
        private readonly Fixture _fixture;
        public MonkeyTestAttribute(Type classType, string methodName, bool allPossibleCombinations = false)
        {
            try
            {
                _allPossibleCombinations = allPossibleCombinations;
                _fixture = new Fixture
                {
                    RepeatCount = 3
                };
                _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
                _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not find method '{methodName}' on type {classType.FullName}",
                    nameof(methodName), ex);
            }
        }

        public IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var methodArgumentTypes = testMethod.GetParameters().Select(x => x.ParameterType).ToList();
            return MonkeyTestUtils.GetData(methodArgumentTypes, _allPossibleCombinations, _fixture);
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return $"{methodInfo.Name}({string.Join(",", data)})";
        }
    }
}
