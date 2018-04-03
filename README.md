<center>
    <img src="https://raw.githubusercontent.com/abdulbeard/monkey_testing/master/Curious_George.png"/>
</center>
<h1 style="text-align: center">Monkey Testing<h1>

## Table of Contents
* [Introduction](#intro)
* [Use Cases](#usecases)
* [How To Use](#howTo)
    * [Installation](#installation)
    * [Writing Tests](#writingTests)
        * [NUnit](#nunitTests)
        * [XUnit](#xunitTests)
        * [MSTest](#msTests)
    * [All Possible Combinations](#allPossibleCombinations)
* [Basic Variations](#basicVariations)
* [Examples](#examples)

<a id="intro"></a>
## Introduction
[Monkey Testing](https://en.wikipedia.org/wiki/Monkey_testing) and Random Testing are not new concepts. They are useful for testing your code beyond your cognitive blindspots and assumptions for how it is meant to be used/executed.
This library allows you to test your functions with lots of random, ideally troublesome inputs. It tries to approximate easy-to-overlook values of primitive and complex types, that are arguments to the functions you want to test.

The nuget package [Curious George](https://www.nuget.org/packages/CuriousGeorge) is named after the popular [children's books character](https://en.wikipedia.org/wiki/Curious_George).

<a id="usecases"></a>
## Use Case
Common use cases for monkey testing are when you're operating on user-provided input (webform inputs), or parsing/transforming data from one representation to another e.g. serialized objects, URIs represented as strings, converting ints to longs, converting decimals to floats to doubles etc. Outside of primitive C# types, this library also works with enums, and custom class types.

[AutoFixture](https://github.com/AutoFixture/AutoFixture) is used for creating random instances of user-defined classes.

<a id="howTo"></a>
## How To Use
Let's say you have a function to test:
``` csharp
    public class CodeToTest
    {
        public int FunctionToTest1(int quantity, int count, int multiplier)
        {
            return quantity * count * multiplier;
        }
    }
```
And you write all the regular tests you'd want to write, to make sure you're calculating the right `product` of the three input arguments. But let's say you also want to make sure your code works with null, empty, min and max values of your input arguments.

<a id="installation"></a>
### Installation
Start by installing the nuget package on your tests project.
You can install this library via nuget as follows:
> Install-Package CuriousGeorge

or via Nuget Package Manager in Visual Studio by searching for '`CuriousGeorge`'.

This library supports [MSTest](https://en.wikipedia.org/wiki/Visual_Studio_Unit_Testing_Framework), [xunit](https://xunit.github.io/) and [nunit](http://nunit.org/) testing frameworks. However, adding support for any others is hardly difficult.

<a id="writingTests"></a>
### Writing Tests
Once you have the package installed, then you can test your `FunctionToTest1` as follows:

---
<a id="nunitTests"></a>
For NUnit:
``` csharp
    public class NUnitTests
    {
        [Test]
        [TestCaseSource(typeof(MonkeyTestCaseSource), nameof(MonkeyTestCaseSource.GetData), new object[] {typeof(NUnitTests), nameof(Test1), true})]
        public void Test1(int a, int b, int c)
        {
            var result = new CodeToTest().FunctionToTest1(a, b, c);
            Assert.True(result <= int.MaxValue);
            Assert.Pass();
        }
    }
```
The `TestCaseSource` attribute is part of Nunit. By specifying the test case source as `MonkeyTestCaseSource`, and the function that returns the test dataset as `MonkeyTestCaseSource.GetData`, we are letting Nunit know where to go to find the datasets to run the test with. Furthermore, the `object[]` is the list of arguments that will be passed to `GetData`. In here, the method on which the `TestCaseSource` attribute is applied is being identified. CuriousGeorge uses [Reflection](#https://docs.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/reflection) to figure out what object types to create monkey tests datasets for. E.g. in this case, `Test1(int a, int b, int c)` is being identified by using `typeof(Tests)` (for the class) and `nameof(Test1)` (for the function).

---
<a id="xunitTests"></a>
For xunit:
``` csharp
    public class XUnitTests
    {
        [Theory]
        [MonkeyTest(typeof(XUnitTests), allPossibleCombinations: false)]
        public void Test1(int a, int b, int c)
        {
            var result = new CodeToTest().FunctionToTest1(a, b, c);
            Assert.InRange(result, int.MinValue, int.MaxValue);
        }
    }
```
In the case of xunit, all you have to identify is the type of the current class. The `MonkeyTest` attribute inherits from the xunit [ClassDataAttribute](https://github.com/xunit/xunit/blob/master/src/xunit.core/ClassDataAttribute.cs).

---
<a id="msTests"></a>
For MSTest:
``` csharp
public class MSTests
    {
        [DataTestMethod]
        [MonkeyTest(typeof(MSTests), nameof(Test1), true)]
        public void Test1(int a, int b, int c)
        {
            var result = new CodeToTest().FunctionToTest1(a, b, c);
            Assert.InRange(result, int.MinValue, int.MaxValue);
        }
    }
```
In the case of MSTest, the `MonkeyTest` attribute implements the interface [ITestDatasource](https://github.com/Microsoft/testfx/blob/master/src/Adapter/PlatformServices.Interface/ITestDataSource.cs), which is used by MSTest and VisualStudio to identify test data sources.

<a id="allPossibleCombinations"></a>
### All Possible Combinations
The four basic variations of values for a given type are the following:
* Null
* Default
* Min
* Max

For example, for the input argument type `int`, Null = `0`, Default = `default(int)`, Min = `int.MinValue` and Max = `int.MaxValue`. Now for a function like `FunctionToTest1(int a, int b, int c)` which takes in three ints, each of which can have one of 4 values - all possible combinations of values is a total of 64 combinations (4 x 4 x 4 = 64).
Since int is a [value type](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/value-types), the Null and Default values for it are both `0`. 
CuriousGeorge will make sure that the result set returned is distinct, and that any given result set is never repeated - which brings the total number of combinations down to 27 (3 x 3 x 3 = 27).
`allPossibleCombinations` is the last argument for the monkey test dataset creation.

<a id="basicVariations"></a>
## Variations

| Types        | Null          | Default       | Min           | Max       |
| ------------- |:-------------:|:-------------:|:-------------:|:-------------:|
| byte           | 0 | default(byte) |byte.MinValue|byte.MaxValue|
| sbyte           | 0 | default(sbyte) |sbyte.MinValue|sbyte.MaxValue|
| int           | 0 | default(int) |int.MinValue|int.MaxValue|
| uint           | 0 | default(uint) |uint.MinValue|uint.MaxValue|
| short           | 0 | default(short) |short.MinValue|short.MaxValue|
| ushort           | 0 | default(ushort) |ushort.MinValue|ushort.MaxValue|
| long           | 0 | default(long) |long.MinValue|long.MaxValue|
| ulong           | 0 | default(ulong) |ulong.MinValue|ulong.MaxValue|
| float           | 0.0f | default(float) |float.MinValue|float.MaxValue|
| double           | 0.0d | default(double) |double.MinValue|double.MaxValue|
| char           | '\0' | default(char) |char.MinValue|char.MaxValue|
| bool           | false | default(bool) |false|false|
| string           | null | default(string) |string.Empty|[GetStringMax()](https://github.com/abdulbeard/monkey_testing/blob/c3337a3240fae6e4fca573f24f968cc5195b4f83/MonkeyTesting/DataVariationsByType.cs#L135)|
| decimal           | 0.0M | default(decimal) |decimal.MinValue|decimal.MaxValue|
| DateTime           | DateTime.MinValue | default(DateTime) |DateTime.MinValue|DateTime.MaxValue|



<a id="examples"></a>
## Examples
* For inspiration on how to use CuriousGeorge in MSTest, check [here](https://github.com/abdulbeard/monkey_testing/tree/master/MonkeyTesting.Tests.Mstest)
* For XUnit, check [here](https://github.com/abdulbeard/monkey_testing/tree/master/MonkeyTesting.Tests.Xunit)
* And for NUnit, check [here](https://github.com/abdulbeard/monkey_testing/tree/master/MonkeyTesting.Tests.NUnit)



