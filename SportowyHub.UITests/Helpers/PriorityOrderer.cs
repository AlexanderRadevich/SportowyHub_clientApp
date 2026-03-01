using Xunit.Sdk;
using Xunit.v3;

namespace SportowyHub.UITests.Helpers;

public sealed class PriorityOrderer : ITestCaseOrderer
{
    public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
        where TTestCase : notnull, ITestCase
    {
        var sorted = new SortedDictionary<int, List<TTestCase>>();

        foreach (var testCase in testCases)
        {
            var priority = int.MaxValue;

            if (testCase is IXunitTestCase xunitTestCase)
            {
                var attr = xunitTestCase.TestMethod.Method
                    .GetCustomAttributes(typeof(TestPriorityAttribute), true)
                    .OfType<TestPriorityAttribute>()
                    .FirstOrDefault();

                if (attr is not null)
                {
                    priority = attr.Priority;
                }
            }

            if (!sorted.TryGetValue(priority, out var list))
            {
                list = [];
                sorted[priority] = list;
            }

            list.Add(testCase);
        }

        return sorted.Values.SelectMany(l => l).ToList();
    }
}
