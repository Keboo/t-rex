using System;
using System.Collections.Generic;
using System.CommandLine.Rendering.Views;
using System.Linq;
using TRexLib;

namespace TRex.CommandLine.Views
{
    public sealed class TestResultsView : StackLayoutView
    {
        public TestResultsView(bool hideTestOutput, IEnumerable<TestResult> testResults)
        {
            if (testResults == null) throw new ArgumentNullException(nameof(testResults));

            var groupings = testResults
                .GroupBy(result => result.Outcome)
                .Select(
                    byOutcome => new
                    {
                        Outcome = byOutcome.Key,
                        Items = byOutcome
                            .GroupBy(result => result.Namespace)
                            .Select(
                                byNamespace =>
                                    new
                                    {
                                        Namespace = byNamespace.Key,
                                        Items = byNamespace
                                            .GroupBy(result => result.ClassName)
                                            .Select(
                                                byClass => new
                                                {
                                                    ClassName = byClass.Key,
                                                    Items = byClass.Select(g => g)
                                                })
                                    })
                    });

            foreach (var groupingByOutcome in groupings.OrderBy(t => t.Outcome))
            {
                var durationForOutcome =
                    groupingByOutcome
                        .Items
                        .Sum(ns =>
                            ns.Items
                                .Sum(className =>
                                    className.Items
                                        .Sum(test => test.Duration?.TotalSeconds)));
                var groupView = new ColoredStackLayoutView(GetColorForOutcome(groupingByOutcome.Outcome));
                Add(groupView);

                groupView.Add(new StackLayoutView(Orientation.Horizontal)
                {
                    new ContentView($"{groupingByOutcome.Outcome.ToString().ToUpper()}     "),
                    new DurationView(durationForOutcome)
                });

                foreach (var groupingByNamespace in groupingByOutcome.Items)
                {
                    var durationForNamespace =
                        groupingByNamespace
                            .Items
                            .Sum(className => className.Items
                                                       .Sum(test => test.Duration?.TotalSeconds));

                    groupView.Add(new StackLayoutView(Orientation.Horizontal)
                    {
                        new ContentView($"  {groupingByNamespace.Namespace}     "),
                        new DurationView(durationForNamespace)
                    });

                    foreach (var groupingByClassName in groupingByNamespace.Items)
                    {
                        var durationForClass =
                            groupingByClassName.Items
                                               .Sum(className => className.Duration?.TotalSeconds);

                        groupView.Add(new StackLayoutView(Orientation.Horizontal)
                        {
                            new ContentView($"    {groupingByClassName.ClassName}     "),
                            new DurationView(durationForClass)
                        });

                        foreach (TestResult result in groupingByClassName.Items)
                        {
                            groupView.Add(new StackLayoutView(Orientation.Horizontal)
                            {
                                new ContentView("      "),
                                new TestResultView(hideTestOutput, result)
                            });
                        }
                    }
                }

            }
        }


        private static System.ConsoleColor GetColorForOutcome(TestOutcome outcome)
        {
            switch (outcome)
            {
                case TestOutcome.NotExecuted:
                case TestOutcome.Inconclusive:
                case TestOutcome.Pending:
                    return System.ConsoleColor.Yellow;
                case TestOutcome.Failed:
                    return System.ConsoleColor.Red;
                case TestOutcome.Passed:
                    return System.ConsoleColor.Green;
                case TestOutcome.Timeout:
                    return System.ConsoleColor.Magenta;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}