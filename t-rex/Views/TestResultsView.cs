using System;
using System.Collections.Generic;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Linq;
using TRexLib;

namespace TRex.CommandLine.Views
{
    public sealed class TestResultsView : StackLayoutView
    {
        public TestResultsView(bool hideTestOutput, TestResultSet testResults)
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
                var groupView = new ColoredStackLayoutView(groupingByOutcome.Outcome.GetColorForOutcome());
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
                        new TContentView($"  {groupingByNamespace.Namespace}     "),
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

            Add(new SummaryView(testResults.Passed.Count, testResults.Failed.Count, testResults.NotExecuted.Count));
        }
    }

    public class TContentView : ContentView
    {
        public TContentView(string content)
            : base(content)
        {
            
        }

        public override Size Measure(ConsoleRenderer renderer, Size maxSize)
        {
            var rv =  base.Measure(renderer, maxSize);
            return rv;
        }

        public override void Render(ConsoleRenderer renderer, Region region)
        {
            base.Render(renderer, region);
        }
    }
}